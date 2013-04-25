using System.Web;
using System.Web.Mvc;
using System.Web.Routing;



namespace HAC
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {

        public static string GetServerPath()
        {
            return HttpRuntime.AppDomainAppVirtualPath;
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
           "LatestComments", // Nombre de ruta
           "Comment/Latest/{top}", // URL con parámetros
           new { controller = "Comment", action = "Latest", top = 10 } // Valores predeterminados de parámetro
      );

            routes.MapRoute(
            "ImageView", // Nombre de ruta
            "ImageView", // URL con parámetros
            new { controller = "Image", action = "View", ImageVPath = UrlParameter.Optional } // Valores predeterminados de parámetro
          );


            routes.MapRoute(
            "ThumbnailView", // Nombre de ruta
            "ThumbnailView/{width}/{height}", // URL con parámetros
            new { controller = "Image", action = "Thumbnail", width = "100", height = "100", ImageVPath = UrlParameter.Optional } // Valores predeterminados de parámetro
            );


            routes.MapRoute(
            "ImageMain", // Nombre de ruta
            "Image", // URL con parámetros
            new { controller = "PGImage", action = "Index", ImageVPath = UrlParameter.Optional } // Valores predeterminados de parámetro
            );


            routes.MapRoute(
                            "Image", // Nombre de ruta
                            "Image/{id}", // URL con parámetros
                            new { controller = "PGImage", action = "Index", id = UrlParameter.Optional } // Valores predeterminados de parámetro
                            );
            routes.MapRoute(
             "MainFolderList", // Nombre de ruta
             "Main/Folder/List/{folder}", // URL con parámetros
             new { controller = "PGFolder", action = "List", folder = UrlParameter.Optional } // Valores predeterminados de parámetro
         );


            routes.MapRoute(
                "FolderList", // Nombre de ruta
                "Folder/List/{folder}", // URL con parámetros
                new { controller = "PGFolder", action = "List", folder = UrlParameter.Optional } // Valores predeterminados de parámetro
            );


            routes.MapRoute(
               "PhotosCategoryList", // Nombre de ruta
               "PHOTOS/Category/List/{iCat}", // URL con parámetros
               new { controller = "Category", action = "List", iCat = 0 } // Valores predeterminados de parámetro
           );

            routes.MapRoute(
              "CategoryList", // Nombre de ruta
              "Category/List/{iCat}", // URL con parámetros
              new { controller = "Category", action = "List", iCat = 0 } // Valores predeterminados de parámetro
          );

            routes.MapRoute(
            "Default", // Route name
            "{controller}/{action}/{id}", // URL with parameters
            new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
        );





        }

        protected void Application_Start()
        {

            RegisterRoutes(RouteTable.Routes);


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