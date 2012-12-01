using System.Configuration;

namespace MvcApplicationWithVersioning.Configuration
{
    public class AppConfiguration : IAppConfiguration
    {
        private const string ControllerVersionKeyPattern = "Controller{0}Postfix";
        private const string ViewVersionKeyPattern = "View{0}FolderPostfix";

        public string ApiVersionKey { get { return "version"; } }
        public string ControllerKey { get { return "controller"; } }
        public string DefaultControllerVersion { get { return ConfigurationManager.AppSettings["DefaultControllerVersion"]; } }
        public string DefaultViewsVersion { get { return ConfigurationManager.AppSettings["DefaultViewsVersion"]; } }

        /// <summary>
        /// If Controller matches this rule - RouteControllerFactory will be used instead of default one
        /// (this factory handles API versioning)
        /// </summary>
        public string ControllerNameRegexForEnablingCustomControllerFactory { get { return ConfigurationManager.AppSettings["ControllerNameRegexForEnablingCustomControllerFactory"]; } }

        /// <summary>
        /// In case if Type is not found will be returned null
        /// </summary>
        public string TryGetControllerPostfix(string version)
        {
            return ConfigurationManager.AppSettings[string.Format(ControllerVersionKeyPattern, version)];
        }

        /// <summary>
        /// In case if Postfix is not found will be returned null
        /// </summary>
        public string TryGetViewFolderPostfix(string version)
        {
            return ConfigurationManager.AppSettings[string.Format(ViewVersionKeyPattern, version)];
        }
    }
}