using System.Web.Mvc;
using System.Web.Routing;
using HAC.Domain;


namespace HAC
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
     

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
           // Initializer.RegisterBytecodeProvider();
           RegisterRoutes(RouteTable.Routes);
            //log4net.Config.XmlConfigurator.Configure();
           DataConfig.EnsureStartup();

            AreaRegistration.RegisterAllAreas();
            RegisterGlobalFilters(GlobalFilters.Filters);
            

        }


        //protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        //{
        //    HttpCookie authCookie = Context.Request.Cookies[FormsAuthentication.FormsCookieName];
        //    if (authCookie == null || authCookie.Value == "")
        //        return;

        //    FormsAuthenticationTicket authTicket;
        //    try
        //    {
        //        authTicket = FormsAuthentication.Decrypt(authCookie.Value);
        //    }
        //    catch
        //    {
        //        return;
        //    }

        //    // retrieve roles from UserData
        //    if (authTicket != null)
        //    {
        //        string[] roles = authTicket.UserData.Split(';');

        //        if (Context.User != null)
        //            Context.User = new GenericPrincipal(Context.User.Identity, roles);
        //    }
        //}
    }
}