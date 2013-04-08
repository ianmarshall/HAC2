using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HAC.Controllers
{
    public class StIves10kController : Controller
    {


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Info()
        {
            return View("Index");
        }

        public ActionResult Entry()
        {
            return View("Index");
        }

    }
}
