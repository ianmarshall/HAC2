using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Caching;
using System.Diagnostics;

namespace HAC.Models.Repositories
{
    /// <summary>
    /// Summary description for Repository
    /// </summary>
    /// 
    public class GalleryRepository
    {

        private PGFolder _RootFolder = null;
        public PGFolder RootFolder { get { return _RootFolder;  } }
        public List<Comment> Comments { get; set; } //list of all comments (for faster access)
        public List<PGFolder> Folders { get;set;} //Folder with hierarchy
        public List<PGFolder> AllFolders { get; set; } //List of all folders (for faster access)
        public List<PGImage> Images { get; set; } //list of all images (for faster access)

        private string _basePhysicalPath=null;

        private static object cacheLock = new object();

        public GalleryRepository(string basePhysicalPath)
        {
            Comments = new List<Comment>();
            Folders = new List<PGFolder>();
            AllFolders = new List<PGFolder>();
            Images = new List<PGImage>();

            _basePhysicalPath = basePhysicalPath;

            //initialize
            PGFolder repFolder = new PGFolder();
            repFolder.PhysicalPath = _basePhysicalPath;

            string cache = "cache_rootfolders";
            List<string> filenames_cache = new List<string>();
            FillSubFoldersRecursive(ref repFolder, new PGFolderNameASCComparer(), out filenames_cache);
            filenames_cache.Add(Configuration.ConfigurationFilePhysicalPath);

            //cache
            lock (cacheLock)
            {
                HttpContext.Current.Cache.Add(cache, repFolder,
                    new System.Web.Caching.CacheDependency(filenames_cache.ToArray<string>()), System.Web.Caching.Cache.NoAbsoluteExpiration,
                    new TimeSpan(0, 20, 0), System.Web.Caching.CacheItemPriority.Normal, new System.Web.Caching.CacheItemRemovedCallback(RemovedCallback));
            }

            Folders = repFolder.SubFolders;
            _RootFolder = repFolder;
        }


        private void FillSubFoldersRecursive(ref PGFolder folder, PGFolderComparer defaultSortingComparer, out List<string> filenames_cache)
        {
            filenames_cache = new List<string>();

            DirectoryInfo directory = new DirectoryInfo(folder.PhysicalPath);
            if (!directory.Exists)
            {
                throw new DirectoryNotFoundException(string.Format("Directory {0} not found", directory));
            }
            else
            {                                    
                //1. READ INFO FILE
                folder.ReadFolder();

                Images.AddRange(folder.Images); //cache images for easy access
                Comments.AddRange(folder.Comments); //cache comments for easy access.

                //2. LOAD SUB FOLDERS from disk (RECURSIVE)
                folder.SubFolders = new List<PGFolder>();

                defaultSortingComparer = PGFolderComparer.GetComparerBySortAction(folder.SortAction, defaultSortingComparer);

                //3. subfolder recursive
                foreach (DirectoryInfo subDirectory in directory.GetDirectories())
                {
                    if (!IsDirectoryIgnored(subDirectory.FullName))
                    {
                        PGFolder subfolder = new PGFolder() { Name = subDirectory.Name, PhysicalPath = subDirectory.FullName };
                        subfolder.ParentFolder = folder;
 
                        List<string> filename_cache_subfolder = new List<string>();                        
                        FillSubFoldersRecursive(ref subfolder, defaultSortingComparer,  out filename_cache_subfolder);
                        filenames_cache.AddRange(filename_cache_subfolder);
                        folder.SubFolders.Add(subfolder);
                        folder.NumberNestedImages += subfolder.NumberNestedImages; //todo refactor, inside pgfolder.
                    }
                }                
                
                //4. Sorting?
                folder.SortSubFolders(folder.SortAction, ref defaultSortingComparer);
                
                AllFolders.Add(folder); //index for faster access

                filenames_cache.Add(folder.FolderInfoFileLocation());
            }

            //5. WRITE if necessary
            if (!folder.ExistFolderInfoFile())
                folder.WriteFolderFileInfo();                        
        }

        public void RemovedCallback(String k, Object v, CacheItemRemovedReason r)
        {
            Trace.Write("Key " + k + " removed from cache");
            Console.Write("Key " + k + " removed from cache");
        }
      
        /// <summary>
        /// Gets repository folder based on virtual Path
        /// </summary>
        /// <param name="vfolderpath"></param>
        /// <returns></returns>
        public  PGFolder GetFolderFromVPath(string vfolderpath)
        {
            if (vfolderpath == "default")
            {
                PGFolder found = Folders.FirstOrDefault();
                return found;
            }

            foreach (PGFolder folder in Folders) {
                PGFolder found=folder.SearchSubFolderRecursiveByVPath(vfolderpath);
                if (found != null)
                    return found;
            }
            return null;
        }
   
        public ICollection<Comment> GetByImage(string ImageVPath)
        {
            PGImage image = GetImageFromVPath(ImageVPath);
            return image.Comments;
        }

        public void UpdateImageInfo(PGImage image){
            //save info file.
            PGFolder folder = image.ParentFolder;
            if (folder == null)
            {
                throw new Exception("parent folder doesn't exist");
            }
            folder.WriteFolderFileInfo();
        }


        public PGImage GetImageFromVPath(string ImageVPath)
        {
            
            PGFolder folder = GetFolderFromVPath(Util.GetVirtualFolderPathFromImagePath(ImageVPath));
            foreach (PGImage image in folder.Images){
                if (Util.IsSameVpath(image.VPath,ImageVPath)) {
                    return image;
                }
            }
            return null;         
        }

      

        #region Comments
        public Comment GetComment(string ID)
        {
            return Comments.Where(c => c.ID == ID).FirstOrDefault();   
        }

        public List<Comment> GetLatestComments(int top)
        {
            return Comments.OrderByDescending(c => c.TimeStamp).Take(top).ToList();
        }

        public void  AddComment(string imageVPath, Comment comment)
        {
            PGImage image = this.GetImageFromVPath(imageVPath);
            if (image == null)
                throw new ArgumentException("image not found");

            image.Comments.Add(comment);
            image.ParentFolder.WriteFolderFileInfo();
        }

        public void DeleteComment(Comment comment)
        {
            PGImage image = this.GetImageFromVPath(comment.ImageVPath);
            if (image == null)
                throw new ArgumentException("image not found");

            image.Comments.Remove(comment);
            image.ParentFolder.WriteFolderFileInfo();
        }
        #endregion




        #region Get Next, prev image
        public  PGImage GetPreviousImage(PGImage image)
        {
            //where is this image?
            if (image.ParentFolder != null)
            {

                PGFolder parentFolder = image.ParentFolder;
//                List<PGImage> images = GetImagesFromDiskFolderByVirtualPath(parentFolder);

                PGImage prevImg = null;
                foreach (PGImage img in parentFolder.Images)
                {
                    if (prevImg != null)
                    {
                        if (img.PhysicalPath == image.PhysicalPath)
                        {
                            return prevImg;
                        }
                    }
                    prevImg = img;
                }
            }

            return null;
        }


        public  PGImage GetNextImage(PGImage image)
        {
            if (image.ParentFolder != null)
            {
                PGFolder parentFolder = image.ParentFolder;
                //List<PGImage> images = GetImagesFromDiskFolderByVirtualPath(parentFolder);

                PGImage nextImg = null;
                bool getNext = false;
                foreach (PGImage img in parentFolder.Images)
                {
                    if (getNext)
                        return img;

                    if (img.PhysicalPath == image.PhysicalPath)
                    {
                        getNext = true;
                    }
                }
            }
            return null;
        }
        #endregion

        

        #region Helper functions
      


        private  bool IsDirectoryIgnored(string directoryPath)
        {
            string[] ignorefolders = Configuration.GetConfiguration().IgnoreFolders;
            if (ignorefolders.Length == 0)
                return false;

            return ignorefolders.Any(s => directoryPath.IndexOf(s) > -1);
        }

        #endregion


    }
}