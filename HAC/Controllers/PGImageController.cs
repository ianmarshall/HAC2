using HAC.Domain;
using HAC.Domain.Repositories;
using HAC.Models;
using HAC.Models.Repositories;
using System.Web;
using System.Web.Mvc;

namespace HAC.Controllers
{
    public class PGImageController : BaseController
    {
        private bool ValidName(string val)
        {
            if (val.IndexOf('<') > -1)
                return false;

            if (val.IndexOf('>') > -1)
                return false;

            return true;
        }

        [Authorize(Roles = "Admin")]
        public JsonResult EditImageInfo(FormCollection formValues)
        {
            GalleryRepository rep = this.GetGalleryRepository();
            PGImage image = rep.GetImageFromVPath(formValues["ImageVPathEditImageInfo"]);
            if (image == null)
            {
                Response.StatusCode = 400;
                return Json(new { error = "Image not found at " + formValues["ImageVPath"] });
            }

            image.FriendlyName = formValues["ImageFriendlyName"];
            image.ImageDescr = formValues["ImageDescription"];

            if (!ValidName(image.FriendlyName) || !ValidName(image.ImageDescr))
            {
                Response.StatusCode = 400;
                return Json(new { error = "Image not found at " + formValues["ImageVPath"] });
            }

            rep.UpdateImageInfo(image);

            return Json(new { ImageFriendlyName = image.FriendlyName, ImageDescr = image.ImageDescr }, JsonRequestBehavior.AllowGet);
        }


        //[OutputCache(Duration = 0, VaryByParam = "ImageVPath")] //don't use cache here (ajax and html responses)
        public ActionResult Index(int id)
        {
            PhotoRepository rep = this.GetPhotoRepository();

            ////ImageVPath = HttpUtility.UrlDecode(ImageVPath);
            ////if (ImageVPath[0] == '/')
            ////{
            ////    ImageVPath = ImageVPath.Substring(1);
            ////}

            pic_images image = rep.GetImage(id);
            //if (image != null)
            //{
            //    string cacheKey = "exif" + id;
            //    if (HttpContext.Cache[cacheKey] = null)
            //    {
            //        image.EXIF = HttpContext.Cache[cacheKey] as EXIF;
            //    }
            //    else
            //    {
            //        //exif info
            //        ExifReader r = new ExifReader(image.PhysicalPath);  //file is String
            //        r.FillPGImage(ref image);
            //        HttpContext.Cache.Add(cacheKey, image.EXIF, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, 10, 0), CacheItemPriority.Normal, null);
            //    }

            //    PGImage imagePrev=null; //prev img?
            //    PGImage nextImg=null; //next img?
            //    if (image.ParentFolder != null) {
            //        imagePrev = rep.GetPreviousImage(image);
            //        nextImg = rep.GetNextImage(image);      
            //    }

            var imageJson = new
            {
                //ImageMainPage = image.MainPageFullUrl,
                ImageVPath = Util.GetApplicationFullUrlWithoutLastSlash() + "/ImageView?ImageVPath=" + HttpUtility.UrlEncode(image.PIC_IMAGE),
                ImageFriendlyName = image.PIC_NAME,
                ImageDescr = image.PIC_DESC,
                Name = image.PIC_NAME,
                //NextThumbnail = nextImg == null ? "" : nextImg.ImageThumbnailFullUrl,
                //NextImage = nextImg == null ? "" : nextImg.ImageFullUrl,
                //PrevThumbnail = imagePrev == null ? "" : imagePrev.ImageThumbnailFullUrl,
                //PrevImage = imagePrev == null ? "" : imagePrev.ImageFullUrl,
                //UrlThumbnail = image.ImageThumbnailFullUrl,
                //Url = image.ImageFullUrl,
                // Comments = image.Comments.OrderByDescending(c => c.TimeStamp),
                // EXIF = image.EXIF,
            };

            if (Request.IsAjaxRequest())
                return Json(imageJson, JsonRequestBehavior.AllowGet);
            else
            {
                ViewBag.SelectedImage = "Image/" + image.PIC_ID;
                ViewBag.Categories = rep.GetCategories();
                ViewBag.SelectedCategory = Util.GetApplicationFullUrlWithoutLastSlash() + "/PHOTOS/Category/List/" +
                                           image.PIC_CAT;
                return View("../PHOTOS/Index");
            }
        }
        //else
        //    return Content("Image not found at " + id);

    }

}

