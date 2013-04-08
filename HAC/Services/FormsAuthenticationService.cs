using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace HAC.Services
{
    public class FormsAuthenticationService
    {
        public void SignIn(string userName, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(userName))
                throw new ArgumentException("Value cannot be null or empty.", "userName");

            FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
            HttpContext.Current.Session["UserName"] = userName;
           
        }

        public void SignOut()
        {

            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Abandon();
        }
    }
}