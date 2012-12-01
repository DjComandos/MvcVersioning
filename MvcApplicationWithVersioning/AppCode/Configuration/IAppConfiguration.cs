namespace MvcApplicationWithVersioning.AppCode.Configuration
{
    public interface IAppConfiguration
    {
        string ApiVersionKey { get; }
        string ControllerKey { get; }
        string DefaultControllerVersion { get; }
        string DefaultViewsVersion { get; }

        /// <summary>
        /// If Controller matches this rule - RouteControllerFactory will be used instead of default one
        /// (this factory handles API versioning)
        /// </summary>
        string ControllerNameRegexForEnablingCustomControllerFactory { get; }

        /// <summary>
        /// In case if Type is not found will be returned null
        /// </summary>
        string TryGetControllerPostfix(string version);

        /// <summary>
        /// In case if Postfix is not found will be returned null
        /// </summary>
        string TryGetViewFolderPostfix(string version); 
    }
}