using HAC.Models;
using System;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace MVC.Image.Resize.Helpers
{

    public class ImageController : Controller
    {
        private const int duration = 300;


        private void SetCache(HttpResponseBase response, string filepath)
        {
            response.Cache.SetCacheability(HttpCacheability.Public);
            Response.Cache.SetSlidingExpiration(true);
            response.Cache.SetExpires(Cache.NoAbsoluteExpiration);
            Response.AddFileDependency(filepath);
            //Response.AppendHeader("Content-Length", imageBytes.Length.ToString());
            response.ContentType = "image/jpeg";

            DateTime dt = DateTime.Now.AddDays(10);
            response.Cache.SetMaxAge(new TimeSpan(dt.ToFileTime()));
            response.Cache.SetExpires(dt);

            response.Cache.SetLastModifiedFromFileDependencies();
        }


        /// <summary>
        /// Return an image from repository based on virtual path.
        /// Returns streamed image
        /// </summary>
        /// <param name="ImageVPath"></param>
        /// <returns></returns>
        ///        
        public ActionResult View(string ImageVPath)
        {
           
            if (ImageVPath == null)
                return Content("Error: (empty parameter)");
            
            string basePath = Configuration.GetPicturesPhysicalPath;
            string FilePath = Path.Combine(basePath, Util.CleanVPath(ImageVPath));

            if (System.IO.File.Exists(FilePath))
            {
                 SetCache(Response, FilePath);
                return base.File(FilePath, "image/jpeg");
            }
            else
            {
                string noimageFilePath = Configuration.GetNoImageFilePath;
                return base.File(noimageFilePath, "image/jpeg");
            }
        }

        //[OutputCache(Duration = 400, VaryByParam = "ImageVPath, width, height")]

        //[HttpGet, FileCache(Duration = 3600)]
        public ActionResult Thumbnail(int width, int height, string ImageVPath)
        {
            Configuration c = Configuration.GetConfiguration();
            string CacheFilePath = "";

            if (true)
            {
                CacheFilePath = Path.Combine(Configuration.GetCacheFolderPhysicalPath, Path.GetFileNameWithoutExtension(ImageVPath) + "_w" + width + "_h" + height + ".jpg");
                if (System.IO.File.Exists(CacheFilePath))
                    SetCache(Response, CacheFilePath);
            }
            return new ThumbnailResult(width, height, CacheFilePath, ImageVPath);
        }
    }
}