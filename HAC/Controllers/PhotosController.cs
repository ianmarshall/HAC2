using HAC.Domain;
using HAC.Domain.Repositories;
using HAC.Models;
using HAC.Models.Repositories;
using ImageResizer;
using System;
using System.Drawing;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace HAC.Controllers
{
    public class PhotosController : BaseController
    {
        //
        // GET: /Photos/

        public ViewResult Index()
        {
            var photoRepository = new PhotoRepository();

            PhotosModel photosModel = new PhotosModel()
                {
                    PicCount = photoRepository.GetImagesCount(),
                    Categorieses = photoRepository.GetCategories()

                };

            return View("IndexPhoto", photosModel);
        }


        [OutputCache(Duration = 10, VaryByParam = "theme")]
        public ActionResult Main()
        {
            string re = Request.RawUrl;
            GalleryRepository rep = this.GetGalleryRepository();
            ViewBag.Folders = rep.Folders;

            var photoRepository = new PhotoRepository();

            //PhotosModel photosModel = new PhotosModel()
            //{
            //    PicCount = photoRepository.GetImagesCount()
            //    //    Categorieses = photoRepository.GetCategories()

            //};

            ViewBag.Categories = photoRepository.GetCategories();


            ViewBag.HtmlContent = rep.RootFolder.GetHtmlContentIfAny(); //initial html content from root folder
            return View("Index");
        }
        [Authorize]
        public ActionResult Add()
        {
            PhotoRepository rep = this.GetPhotoRepository();
            PhotosModel photosModel = new PhotosModel()
               {
                   PicCount = rep.GetImagesCount(),
                   Categorieses = rep.GetCategories(),
                   CatCount = rep.GetCategories().Count,
               };

            return View("Add", photosModel);
        }

        [HttpPost]
        public ActionResult Add(pic_images currentImage, HttpPostedFileBase imageFile)
        {
            // Verify that the user selected a file
            if (imageFile != null && imageFile.ContentLength > 0)
            {
                // extract only the fielname
                currentImage.PIC_IMAGE = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Configuration.GetPicturesPhysicalPath, currentImage.PIC_IMAGE);
                //imageFile.SaveAs(path);

                var settings = new ResizeSettings
                {
                    MaxWidth = 1024,
                    MaxHeight = 768,
                    Format = "jpg"
                };
                var outStream = new MemoryStream();


                Bitmap bitmap = ImageBuilder.Current.Build(imageFile, settings);
                bitmap.Save(path);
                currentImage.PIC_DATED = DateTime.Now;
                currentImage.PIC_HEIGHT = bitmap.Height.ToString();
                currentImage.PIC_WIDTH = bitmap.Width.ToString();
                bitmap.Dispose();

                currentImage.PIC_DATED = DateTime.Now;
                PhotoRepository rep = this.GetPhotoRepository();
                rep.Save(currentImage);

            }

            return RedirectToRoute("ImageMain", new { id = currentImage.PIC_ID });
        }
    }
}
