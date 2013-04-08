using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using HAC.Models.Serialization;
using HAC.Models;
using System.Web.Script.Serialization;

/// <summary>
/// Summary description for Folder
/// </summary>
public class PGFolder
{
    private const string cfolderInfo="folderInfo.inc";
    object locker=new object();

    #region Constructors
    public PGFolder()
    {
        SubFolders = new List<PGFolder>();
        Images = new List<PGImage>();
        Comments = new List<Comment>();
        Security = new SecurityInfo() { PrivateFolder=false};
    }
    #endregion

    [ScriptIgnore]
    public PGFolder ParentFolder
    {
        get;
        set;
    }

    #region Security
    public bool IsPrivateFolder()
    {
        if (Security.PrivateFolder)
            return true;

        PGFolder parent = ParentFolder;
        if (parent == null)
        {            
            return Security.PrivateFolder;
        }
        else {
            if (!parent.Security.PrivateFolder)
            {
                return parent.IsPrivateFolder();
            }
            else {
                return true;
            }
            
        }
    }

    private List<string> GetRecursiveAccessList() {
        List<string> users = Security.UserAccessList;

        PGFolder parent = ParentFolder;
        if (parent == null)
        {
            return users;
        }
        else
        {
            users.AddRange(parent.GetRecursiveAccessList());
        }
        return users;    
    }

    public bool IsUserGranted(string username)
    {
        if (IsPrivateFolder())
        {
            return GetRecursiveAccessList().Contains(username) || Configuration.GetConfiguration().AdminEmails.Contains(username);
        }
        else
            return true;
    }
    #endregion

    public string Name { get; set; }
    public string PhysicalPath {get; set;}
    
    public string VPath{
        get {
            string basePath = Configuration.GetConfiguration().RepositoryPhysicalPath;
            return PhysicalPath.Replace(basePath, "").Replace("\\", "/");
        }
    }

    /// <summary>
    /// Number of images in current folder (not including nested folders)
    /// </summary>
    public int NumberImages {
        get {
            return Images.Count;
        }
    }    

    /// <summary>
    /// Number of images in current folder and nested folders (recursive)
    /// </summary>
    public int NumberNestedImages { get; set; }

    public DateTime TimeStamp { get; set; }
    
    //[ScriptIgnore]
    public List<PGImage> Images { get; set; }
    
    [ScriptIgnore]
    public List<PGFolder> SubFolders{ get; set; }
    
    [ScriptIgnore]
    public List<Comment> Comments { get; set; }

    public int Order { get; set; }
    public SortAction SortAction { get; set; }

    public string GetHtmlContentIfAny() {
        string content = "";
        string contentFile = Path.Combine(PhysicalPath, "content.html");
        if (System.IO.File.Exists(contentFile))
        {
            using (StreamReader sr = new StreamReader(contentFile))
            {
                content = sr.ReadToEnd();
            }
        }
        return content;
    }


    #region Display Helpers

    public string FolderFullUrl
    {
        get
        {
            return Util.GetApplicationFullUrlWithoutLastSlash() + VPath;
        }
    }

    public string FolderInfo //used by treeview
    {
        get
        {
            return (SubFolders.Count > 0) ? SubFolders.Count + " sub., " + NumberNestedImages + " images" : NumberNestedImages + " images";
        }
    }
    #endregion

    public SecurityInfo Security {get;set;} //security as defined for this folder
    public SecurityInfo CalculatedSecurity { get; set; } //takes into consideration parent folders

    #region Search items inside this folder
    public PGFolder SearchSubFolderRecursiveByVPath(string virtualPath) {

        if (Util.IsSameVpath(this.VPath,virtualPath))
            return this;

        foreach(PGFolder folder in this.SubFolders){            
            //Console.Write("Looking for folder " + virtualPath + " in folder " + folder.Name);
            PGFolder f = folder.SearchSubFolderRecursiveByVPath(virtualPath);
            if (f != null)
                return f;
        }
        return null;
    }


    public PGFolder SearchSubFolderByPhysicalPath(string physicalPath)
    {

        if (this.PhysicalPath == physicalPath)
            return this;

        foreach (PGFolder folder in this.SubFolders)
        {
            PGFolder f = folder.SearchSubFolderByPhysicalPath(physicalPath);
            if (f != null)
                return f;
        }
        return null;
    }
    #endregion


    public void SortSubFolders(SortAction sortAction, ref PGFolderComparer defaultcomparer)
    {
        PGFolderComparer comparer = PGFolderComparer.GetComparerBySortAction(sortAction, defaultcomparer);
        SubFolders.Sort(comparer);
        defaultcomparer = comparer;
    }

    #region Helpers
    private List<PGImage> GetImagesFromDiskFolder()
    {
        DirectoryInfo directory = new DirectoryInfo(PhysicalPath);

        List<PGImage> images = new List<PGImage>();
        foreach (FileInfo file in directory.GetFiles())
        {
            #region Add images found on this folder
            if (Util.IsValidImage(file.FullName))
            {
                PGImage image = new PGImage(file.FullName);
                image.FileName = file.Name;
                image.FriendlyName = Path.GetFileNameWithoutExtension(file.Name).Replace("_"," ");
                image.ParentFolder = this;
                images.Add(image);
            }
            #endregion
        }
        return images;
    }
    #endregion

    #region Serialization

    public string FolderInfoFileLocation()
    {
        return Path.Combine(this.PhysicalPath, cfolderInfo);
    }



    public bool ExistFolderInfoFile() {
        return File.Exists(FolderInfoFileLocation());  
    }



    
    /// <summary>
    /// Writes info data for this folder on disk.
    /// </summary>
    public void WriteFolderFileInfo() {
        string filename ="";
        try
        {
            filename = FolderInfoFileLocation();
            PGFolderInfo f = new PGFolderInfo();
            f.Security = this.Security;           

            foreach (PGImage image in this.Images)
            {
                PGImageInfo imageinfo = new PGImageInfo();
                imageinfo.ImageFileName = image.FileName;
                imageinfo.ImageFriendlyName = image.FriendlyName;
                imageinfo.ImageDescription = image.ImageDescr;
                imageinfo.Comments = image.Comments;
                f.ImagesInfo.Add(imageinfo);
            }

            //serialization                        
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(PGFolderInfo));
            lock (locker)
            {
                using (MemoryStream m = new MemoryStream())
                {
                    serializer.Serialize(m, f);
                    //to avoid file corruption if serialization fails
                    using (System.IO.FileStream xmlStream = new System.IO.FileStream(filename, System.IO.FileMode.Create, System.IO.FileAccess.Write))
                    {
                        m.WriteTo(xmlStream);
                    }
                    m.Close();
                }
            }

        
        
        } catch (Exception ex){
            throw new Exception("Error writing folder information file on " + filename, ex);
        }
    }

    /// <summary>
    /// Reads data from xml cache file.
    /// </summary>
    public void ReadFolder()
    {
        string filename = "";

        Images = GetImagesFromDiskFolder();
        NumberNestedImages += NumberImages;

        if (ExistFolderInfoFile())
        {
            try
            {
                lock (locker)
                {
                    filename = Path.Combine(this.PhysicalPath, cfolderInfo);
                    System.Xml.Serialization.XmlSerializer dxml = new System.Xml.Serialization.XmlSerializer(typeof(PGFolderInfo));
                    using (System.IO.FileStream xmlStream = new System.IO.FileStream(filename, System.IO.FileMode.Open, System.IO.FileAccess.Read))
                    {
                        PGFolderInfo info = dxml.Deserialize(xmlStream) as PGFolderInfo;
                        this.Order = info.Order;
                        this.SortAction = info.SortAction;
                        this.Security = info.Security != null ? info.Security : new SecurityInfo();

                        foreach (PGImageInfo imageinfo in info.ImagesInfo)
                        {
                            //search for that image.
                            PGImage image = this.Images.Where(r => r.FileName == imageinfo.ImageFileName).FirstOrDefault();
                            if (image != null)
                            {
                                image.FriendlyName = string.IsNullOrWhiteSpace(imageinfo.ImageFriendlyName) ? image.FileName.Replace("_", " ") : imageinfo.ImageFriendlyName;
                                image.Comments = imageinfo.Comments;

                                foreach (Comment c in image.Comments)
                                {
                                    c.ImageVPath = image.VPath;
                                    Comments.Add(c);
                                }

                                image.ImageDescr = (imageinfo.ImageDescription != null) ? imageinfo.ImageDescription : "";
                            }
                            else
                            {
                                //image referenced in db file not found in file system. 
                                //do nothing, next time file is written to disk will be gone                            
                            }
                        }
                        xmlStream.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error reading folder information file on " + filename, ex);
            }
        }

        if (Configuration.GetConfiguration().ReadBackWardsCompatibilityTxtFiles)
        {
            //backwards compatibility. any txt file associated to picture?
            foreach (PGImage image in this.Images)
            {
                string txtFile = image.PhysicalPath.Replace(Path.GetExtension(image.PhysicalPath), ".txt");
                if (File.Exists(txtFile))
                {
                    using (StreamReader sr = new StreamReader(txtFile))
                    {
                        image.ImageDescr = sr.ReadToEnd();
                    }
                }
            }
        }
    }

    #endregion
}
