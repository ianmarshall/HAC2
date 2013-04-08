using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HAC.Controllers;
using HAC.Models.Repositories;

namespace ASPPhotogalleryMVC.Controllers
{
    public class SearchController : BaseController
    {
        //
        // GET: /Search/
        public ActionResult Search(string q)
        {
            List<PGImage> images= new List<PGImage>();
            if (!string.IsNullOrWhiteSpace(q))
            {
                GalleryRepository rep = this.GetGalleryRepository();

                images = rep.Images.Where(
                    i => 
                        (i.FriendlyName.IndexOf(q, StringComparison.InvariantCultureIgnoreCase) > -1)
                        ||
                        (i.ImageDescr.IndexOf(q, StringComparison.InvariantCultureIgnoreCase) > -1)
                        ||
                        (i.VPath.IndexOf(q, StringComparison.InvariantCultureIgnoreCase) > -1)
                        ||
                        (i.ParentFolder.Name.IndexOf(q, StringComparison.InvariantCultureIgnoreCase) > -1)
                    ).ToList();                
            }

            //let's put them in folders
            List<PGFolder> folders = (from i in images select i.ParentFolder).Distinct().ToList();

            return Json(new { Folders = folders }, JsonRequestBehavior.AllowGet);
        }
    }
}
