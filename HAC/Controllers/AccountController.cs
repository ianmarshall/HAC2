using HAC.Models;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using AccountMembershipService = HAC.Models.AccountMembershipService;

//using HAC.Services;


namespace HAC.Controllers
{
    [HandleError]
    public class AccountController : Controller
    {
        //public FormsAuthenticationService FormsService { get; set; }
        //public AccountMembershipService MembershipService { get; set; }

        //protected override void Initialize(RequestContext requestContext)
        //{
        //    if (FormsService == null) { FormsService = new FormsAuthenticationService(); }
        //    if (MembershipService == null) { MembershipService = new AccountMembershipService(); }

        //    base.Initialize(requestContext);
        //}

        public IFormsAuthenticationService FormsService { get; set; }
        public IMembershipService MembershipService { get; set; }

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

        public ActionResult Register()
        {

            return View("Register");
        }

        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    FormsService.SignIn(model.UserName, false /* createPersistentCookie */);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View("Register", model);
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
                                false,                    // persistent?
                                "Moderator;Member"                        // can be used to store roles
                                                            );

            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);

            var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            HttpContext.Response.Cookies.Add(authCookie);


        }

    }
}
