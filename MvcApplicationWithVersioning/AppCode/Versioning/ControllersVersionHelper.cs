using System;
using System.Web;

namespace MvcApplicationWithVersioning.Versioning
{
    public class ControllersVersionHelper
    {
        public static string ControllersVersionName()
        {
            var context = HttpContext.Current.Request.RequestContext;
            string version = context.RouteData != null && context.RouteData.Values.ContainsKey("version")
                     ? context.RouteData.Values["version"].ToString()
                     : null;

            return version;
        }

        public static ControllersVersion GetUiControllersVersion()
        {
            return ParseVersionFromString(ControllersVersionName());
        }

        private static ControllersVersion ParseVersionFromString(string version)
        {
            ControllersVersion result;
            if (!string.IsNullOrEmpty(version) && Enum.TryParse(string.Format("Version{0}", version), out result))
            {
                return result;
            }

            return ControllersVersion.Unknown;
        } 
    }
}