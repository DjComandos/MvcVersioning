using System;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;
using MvcApplicationWithVersioning.AppCode.Configuration;
using MvcApplicationWithVersioning.AppCode.Reflection;

namespace MvcApplicationWithVersioning.Routing
{
    public class RouteControllerFactory : IControllerFactory
    {
        private readonly IControllerFactory _defaultFactory = new DefaultControllerFactory();
        private readonly IAppConfiguration _configuration = new AppConfiguration();

        public IController CreateController(RequestContext requestContext, string controllerName)
        {
            IController controller = NeedToUseCustomFactory(controllerName, requestContext)
                                         ? CreateControllerInstance(requestContext)
                                         : _defaultFactory.CreateController(requestContext, controllerName);

            return controller;
        }

        public SessionStateBehavior GetControllerSessionBehavior(
            RequestContext requestContext, string controllerName)
        {
            return _defaultFactory.GetControllerSessionBehavior(requestContext, controllerName);
        }

        public void ReleaseController(IController controller)
        {
            _defaultFactory.ReleaseController(controller);
        }

        private IController CreateControllerInstance(RequestContext requestContext)
        {
            var currentVersion = requestContext.RouteData.Values.ContainsKey(_configuration.ApiVersionKey)
                                     ? requestContext.RouteData.Values[_configuration.ApiVersionKey].ToString()
                                     : _configuration.DefaultControllerVersion;

            var controllerVersionedTypeName = string.Format(
                "{0}{1}Controller",
                ControllerName(requestContext),
                _configuration.TryGetControllerPostfix(currentVersion));
            var controllerDefaultTypeName = string.Format(
                "{0}{1}Controller",
                ControllerName(requestContext),
                _configuration.TryGetControllerPostfix(_configuration.DefaultControllerVersion));

            var typeType = TypesFinder.FindTypeInExecutingAssembly(controllerVersionedTypeName) ??
                           TypesFinder.FindTypeInExecutingAssembly(controllerDefaultTypeName);

            var controller = Activator.CreateInstance(typeType) as IController;

            return controller;
        }

        protected string ControllerName(RequestContext requestContext)
        {
            return requestContext.RouteData.Values[_configuration.ControllerKey].ToString();
        }

        private bool NeedToUseCustomFactory(string controllerName, RequestContext requestContext)
        {
            return Regex.IsMatch(controllerName, _configuration.ControllerNameRegexForEnablingCustomControllerFactory)
                && requestContext.RouteData.Values.ContainsKey(_configuration.ApiVersionKey);
        }
    }
}