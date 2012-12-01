namespace MvcApplicationWithVersioning.Versioning
{

    // "Version" here is just enum's version, real name of the version is 1000, 1001, etc.
    public enum ControllersVersion
    {
        Unknown = 0,
        Version1000 = 1,
        Version1001 = 2,
        Version2000 = 3,
    }
}