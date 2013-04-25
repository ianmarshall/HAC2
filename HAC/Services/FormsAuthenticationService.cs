using System;
using System.Web;
using System.Web.Security;

namespace HAC.Services
{
    public class FormsAuthenticationServiceuyuy
    {
        public void SignIn(MembershipUser user, bool createPersistentCookie)
        {
            if (String.IsNullOrEmpty(user.UserName))
                throw new ArgumentException("Value cannot be null or empty.", "userName");

            FormsAuthentication.SetAuthCookie(user.UserName, createPersistentCookie);
            HttpContext.Current.Session["UserName"] = user.UserName;
            HttpContext.Current.Session["UserId"] = user.ProviderUserKey;
        }

        public void SignOut()
        {

            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Abandon();
        }
    }
}