MvcVersioning
=============

example how to apply Versioning for MVC views and controllers:
It is needed to implement custom RouteControllerFactory that implements IControllerFactory,
and custom VersionedRazorViewEngine inherited from RazorViewEngine. All these custom implementations should be registered in Global.asax.
