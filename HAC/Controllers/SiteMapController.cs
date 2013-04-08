using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HAC.Controllers;
using HAC.Models;
using HAC.Models.Repositories;

namespace ASPPhotogalleryMVC.Controllers
{
    public class SiteMapController : BaseController
    {
        //[OutputCache(Duration=60)]
        public PartialViewResult Index()
        {
            ViewBag.SiteUrl = Util.GetApplicationFullUrlWithoutLastSlash();
            GalleryRepository rep = this.GetGalleryRepository();
            return PartialView(rep.AllFolders);
        }

    }
}
