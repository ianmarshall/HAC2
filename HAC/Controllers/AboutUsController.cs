using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HAC.Controllers
{
    public class AboutUsController : Controller
    {
        //
        // GET: /AboutUs/

        public ActionResult Index()
        {
            return View();
        }

        public ViewResult Info()
        {

            return View("Login");
        }

        public ViewResult Membership()
        {
            return View();
        }

        public ViewResult ClubKit()
        {
            return View("Index");
        }

        public ViewResult Contacts()
        {
            return View("Index");
        }

        public ViewResult History()
        {
            return View("Index");
        }

    }
}
