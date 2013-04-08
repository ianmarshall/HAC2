using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HAC.Domain.Repositories;
using HAC.Models;
using System.Web.Caching;
using HAC.Models.Repositories;

namespace HAC.Controllers
{
    public class BaseController : Controller
    {
        protected GalleryRepository GetGalleryRepository() {             
            GalleryRepository rep = new GalleryRepository(Configuration.GetConfiguration().RepositoryPhysicalPath);
            return rep;
        }


        protected PhotoRepository GetPhotoRepository()
        {
            PhotoRepository rep = new PhotoRepository();
            return rep;
        }
    }
}
