using System.Text.RegularExpressions;
using System.Web.Mvc;
using MvcApplicationWithVersioning.Configuration;

namespace MvcApplicationWithVersioning.ViewEngines
{
    public class VersionedRazorViewEngine : RazorViewEngine
    {
        private readonly IAppConfiguration _configuration = new AppConfiguration();

        public VersionedRazorViewEngine()
        {
            AreaViewLocationFormats = new[]
            {
                "~/Areas/{2}/Views/{1}%1%/{0}.cshtml", 
                "~/Areas/{2}/Views/{1}%1%/{0}.vbhtml", 
                "~/Areas/{2}/Views/Shared%1%/{0}.cshtml", 
                "~/Areas/{2}/Views/Shared%1%/{0}.vbhtml"
            };

            AreaMasterLocationFormats = new[]
            {
                "~/Areas/{2}/Views/{1}%1%/{0}.cshtml", 
                "~/Areas/{2}/Views/{1}%1%/{0}.vbhtml", 
                "~/Areas/{2}/Views/Shared%1%/{0}.cshtml", 
                "~/Areas/{2}/Views/Shared%1%/{0}.vbhtml"
            };

            AreaPartialViewLocationFormats = new[]
            {
                "~/Areas/{2}/Views/{1}%1%/{0}.cshtml", 
                "~/Areas/{2}/Views/{1}%1%/{0}.vbhtml", 
                "~/Areas/{2}/Views/Shared%1%/{0}.cshtml", 
                "~/Areas/{2}/Views/Shared%1%/{0}.vbhtml"
            };

            ViewLocationFormats = new[]
            {
                "~/Views/{1}%1%/{0}.cshtml", 
                "~/Views/{1}%1%/{0}.vbhtml", 
                "~/Views/Shared%1%/{0}.cshtml", 
                "~/Views/Shared%1%/{0}.vbhtml"
            };

            MasterLocationFormats = new[]
            {
                "~/Views/{1}%1%/{0}.cshtml", 
                "~/Views/{1}%1%/{0}.vbhtml", 
                "~/Views/Shared%1%/{0}.cshtml", 
                "~/Views/Shared%1%/{0}.vbhtml"
            };

            PartialViewLocationFormats = new[]
            {
                "~/Views/{1}%1%/{0}.cshtml", 
                "~/Views/{1}%1%/{0}.vbhtml", 
                "~/Views/Shared%1%/{0}.cshtml", 
                "~/Views/Shared%1%/{0}.vbhtml"
            };
        }

        protected override IView CreatePartialView(ControllerContext controllerContext, string partialPath)
        {
            return base.CreatePartialView(controllerContext, GetViewPath(controllerContext, partialPath));
        }

        protected override IView CreateView(ControllerContext controllerContext, string viewPath, string masterPath)
        {
            return base.CreateView(
                controllerContext, GetViewPath(controllerContext, viewPath), GetViewPath(controllerContext, masterPath));
        }

        protected override bool FileExists(ControllerContext controllerContext, string virtualPath)
        {
            return base.FileExists(controllerContext, GetViewPath(controllerContext, virtualPath))
                   ||
                   (IsVersioningEnabled(controllerContext) &&
                    base.FileExists(controllerContext, GetViewPath(controllerContext, virtualPath, _configuration.DefaultViewsVersion)));
        }

        private bool IsVersioningEnabled(ControllerContext controllerContext)
        {
            return controllerContext.RouteData.Values.ContainsKey(_configuration.ControllerKey)
                   &&
                   Regex.IsMatch(
                       controllerContext.RouteData.Values[_configuration.ControllerKey].ToString(), _configuration.ControllerNameRegexForEnablingCustomControllerFactory)
                   && controllerContext.RouteData.Values.ContainsKey(_configuration.ApiVersionKey);
        }

        private string GetVersion(ControllerContext controllerContext)
        {
            if (IsVersioningEnabled(controllerContext))
            {
                return controllerContext.RouteData.Values.ContainsKey(_configuration.ApiVersionKey)
                           ? controllerContext.RouteData.Values[_configuration.ApiVersionKey].ToString()
                           : _configuration.DefaultViewsVersion;
            }

            return string.Empty;
        }

        private string GetViewPath(ControllerContext controllerContext, string virtualPath, string version = null)
        {
            if (string.IsNullOrEmpty(virtualPath))
            {
                return string.Empty;
            }

            version = version ?? GetVersion(controllerContext);
            return base.FileExists(controllerContext, virtualPath.Replace("%1%", _configuration.TryGetViewFolderPostfix(version)))
                       ? virtualPath.Replace("%1%", _configuration.TryGetViewFolderPostfix(version))
                       : virtualPath.Replace(
                           "%1%", _configuration.TryGetViewFolderPostfix(_configuration.DefaultViewsVersion));
        }
    }
}