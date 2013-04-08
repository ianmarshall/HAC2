using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using HAC.Models;
using HAC.Services;


namespace HAC.Controllers
{
    public class AccountController : Controller
    {
        public FormsAuthenticationService FormsService { get; set; }
        public AccountMembershipService MembershipService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
            if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

            base.Initialize(requestContext);
        }

        [Authorize]
        public ViewResult Index()
        {
            return View();
        }

        public JsonResult SignIn(string username, string password)
        {
            if (Request.IsAjaxRequest())
            {
                if (MembershipService.ValidateUser(username, password))
                {
                    FormsService.SignIn(username, true);


                    return Json(new { success = true, redirect = Url.Action("Index") });
                }

                return Json(new { error = "The user name or password provided is incorrect" });
            }

            return Json(new { error = "System error" });
        }

        [Authorize]
        public ActionResult SignOut()
        {
            FormsService.SignOut();
            return RedirectToAction("Index", "Home");
        }


        private void Authorize(string userName)
        {
            var authTicket = new FormsAuthenticationTicket(
                                1,                             // version
                                userName,                      // user name
                                DateTime.Now,                  // created
                                DateTime.Now.AddMinutes(20),  // expires
                                true,                    // persistent?
                                "Moderator;Member"                        // can be used to store roles
                                                            );

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            HttpContext.Response.Cookies.Add(authCookie);
        }

    }
}
