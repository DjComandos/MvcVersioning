using System.Web.Mvc;
using System.Web.Routing;
using MvcApplicationWithVersioning.AppCode.Configuration;
using MvcApplicationWithVersioning.AppCode.ViewEngines;
using MvcApplicationWithVersioning.Routing;

namespace MvcApplicationWithVersioning
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
            IAppConfiguration config = new AppConfiguration();
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // let's register out Versioning Route:
            routes.MapRoute(
                "ControllerWithVersion",
                "{controller}/{version}/{action}/{id}",
                new {controller = string.Format(@"\{0}\", config.ControllerNameRegexForEnablingCustomControllerFactory), 
                    action = "ShowLogin", id = UrlParameter.Optional},
                new {version = @"\d{4}"});

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

            // let's register out Versioning Mechanism:

            // register custom controller factory
            ControllerBuilder.Current.SetControllerFactory(typeof(RouteControllerFactory));

            // register custom razor view engine
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new VersionedRazorViewEngine());
            // note: that there is no custom ViewEngine for webforms (it is need to implement it in case it will needed for API versioning)
            ViewEngines.Engines.Add(new WebFormViewEngine());
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }
    }
}