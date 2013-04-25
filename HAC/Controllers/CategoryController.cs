using HAC.Domain;
using HAC.Domain.Repositories;
using HAC.Models;
using ImageResizer;
using System;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace HAC.Controllers
{
    public class CategoryController : BaseController
    {
        //[OutputCache(Duration = 0, VaryByParam = "folder")] //carefull, don't use cache here, different responses whatever is ajax request or not!
        public ActionResult List(int iCat)
        {
            PhotoRepository rep = this.GetPhotoRepository();
            //string vPathFolder = Util.StripFirstSlashFromVirtualPath(HttpUtility.UrlDecode(folder));

            var images = rep.GetCategoryImages(iCat);
            if (images == null)
            {
                Response.StatusCode = 400;
                return Json(new { error = "Error, category not found on id: " + iCat }, JsonRequestBehavior.AllowGet);
            }


            ////check security
            //if (!pgfolder.IsUserGranted(User.Identity.Name)) {
            //        Response.StatusCode = 403;
            //        return Json(new { error = "This is a private folder. You need access to read its content." }, JsonRequestBehavior.AllowGet);                
            //}

            //string content = pgfolder.GetHtmlContentIfAny();

            System.Collections.ArrayList list = new System.Collections.ArrayList();
            foreach (var image in images)
            {
                list.Add(new
                {
                    VPath = Util.GetApplicationFullUrlWithoutLastSlash() + "/ImageView?ImageVPath=" + HttpUtility.UrlEncode(image.PIC_IMAGE),
                    UrlThumbnail = Util.GetApplicationFullUrlWithoutLastSlash() + "/ThumbnailView/120/120?ImageVPath=" + HttpUtility.UrlEncode(image.PIC_IMAGE),
                    Url = HttpContext.Request.ApplicationPath.TrimEnd('/') + "/Image/" + image.PIC_ID,
                    UrlFullImage = Util.GetApplicationFullUrlWithoutLastSlash() + "/ImageView?ImageVPath=" + HttpUtility.UrlEncode(image.PIC_IMAGE),
                    Name = image.PIC_NAME,
                });
            }

            string content = "";

            if (images.Count == 0)
            {
                return View("../PHOTOS/Index");
            }


            if (Request.IsAjaxRequest())
                return Json(new { thumbnails = list, htmlcontent = content }, JsonRequestBehavior.AllowGet);
            else
            {
                ViewBag.SelectedImage = "Image/" + images[0].PIC_ID;
                ViewBag.Categories = rep.GetCategories();
                ViewBag.SelectedCategory = Util.GetApplicationFullUrlWithoutLastSlash() + "/PHOTOS/Category/List/" + iCat;

                return View("../PHOTOS/Index");
            }

        }

        [HttpPost]
        public ActionResult Add(pic_categories currentCategory, HttpPostedFileBase imageFile)
        {
            // Verify that the user selected a file
            if (imageFile != null && imageFile.ContentLength > 0)
            {
                currentCategory.CAT_IMAGE = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                var path = Path.Combine(Configuration.GetPicturesPhysicalPath, currentCategory.CAT_IMAGE);


                var settings = new ResizeSettings
                {
                    MaxWidth = 375,
                    MaxHeight = 500,
                    Format = "jpg"
                };
                Bitmap bitmap = ImageBuilder.Current.Build(imageFile, settings);
                bitmap.Save(path);
                bitmap.Dispose();
                currentCategory.CAT_DATED = DateTime.Now;
                currentCategory.CAT_GROUP = 9;
                PhotoRepository rep = this.GetPhotoRepository();
                rep.Save(currentCategory);
            }

            return RedirectToRoute("PhotosCategoryList", new { id = currentCategory.CAT_ID });
        }
    }
}
