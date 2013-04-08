using HAC.Domain.Repositories;
using HAC.Models;
using HAC.Models.Repositories;
using System.IO;
using System.Web.Mvc;

namespace HAC.Controllers
{
    public class MainController : BaseController
    {
        //
        // GET: /Main/
        [OutputCache(Duration = 10, VaryByParam = "theme")]
        public ActionResult Index(string theme)
        {
            if (!string.IsNullOrWhiteSpace(theme))
            {
                string dir = Server.MapPath("content/themes/" + theme); //make sure theme dir exists
                if (Directory.Exists(dir))
                    Session["theme"] = theme;
            }

            string re = Request.RawUrl;
            GalleryRepository rep = this.GetGalleryRepository();
            ViewBag.Folders = rep.Folders;

            var photoRepository = new PhotoRepository();

            PhotosModel photosModel = new PhotosModel()
            {
                PicCount = photoRepository.GetImagesCount()
            //    Categorieses = photoRepository.GetCategories()

            };

            ViewBag.Categories = photoRepository.GetCategories();


           ViewBag.HtmlContent = rep.RootFolder.GetHtmlContentIfAny(); //initial html content from root folder
            return View("Index");
        }
    }
}
