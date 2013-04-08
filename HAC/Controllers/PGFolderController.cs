using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using HAC.Controllers;
using HAC.Models;
using HAC.Models.Repositories;

namespace HAC.Controllers
{
    public class PGFolderController : BaseController
    { 
        //[OutputCache(Duration = 0, VaryByParam = "folder")] //carefull, don't use cache here, different responses whatever is ajax request or not!
        public ActionResult List(string folder)
        {
            if (string.IsNullOrEmpty(folder)) {
                Response.StatusCode = 400;
                return Json(new { error = "No folder parameter found" }, JsonRequestBehavior.AllowGet);
            }

            GalleryRepository rep = this.GetGalleryRepository();
            string vPathFolder = Util.StripFirstSlashFromVirtualPath(HttpUtility.UrlDecode(folder));

            PGFolder pgfolder = rep.GetFolderFromVPath(vPathFolder);
            if (pgfolder == null) {
                Response.StatusCode = 400;
                return Json(new { error = "Error, folder not found on " + vPathFolder }, JsonRequestBehavior.AllowGet);
            }

            //check security
            if (!pgfolder.IsUserGranted(User.Identity.Name)) {
                    Response.StatusCode = 403;
                    return Json(new { error = "This is a private folder. You need access to read its content." }, JsonRequestBehavior.AllowGet);                
            }

            string content = pgfolder.GetHtmlContentIfAny();

            System.Collections.ArrayList list = new System.Collections.ArrayList();
            foreach (var image in pgfolder.Images) {
                list.Add(new { 
                    VPath = image.VPath,
                    UrlThumbnail = image.ImageThumbnailFullUrl,
                    Url = image.MainPageVirtualUrl, 
                    UrlFullImage = image.ImageFullUrl,
                    Name = image.FileName,
                });
            }

            if (string.IsNullOrWhiteSpace(content) && list.Count==0) {
                if (pgfolder.NumberNestedImages==0)
                    content += "<p>This folder has no images</p>";
                else
                    content += string.Format("<p>This folder has no images, but there are {0} in nested subfolder</p>", pgfolder.NumberNestedImages);
            }


            if (Request.IsAjaxRequest())
                return Json(new { thumbnails = list, htmlcontent = content }, JsonRequestBehavior.AllowGet);
            else
            {
                ViewBag.Folders = rep.Folders;
                ViewBag.SelectedFolder = pgfolder.VPath;
                return View("../Main/Index");
            }
        }
    }
}
