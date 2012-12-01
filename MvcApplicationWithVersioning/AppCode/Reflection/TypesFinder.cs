using System;
using System.Linq;

namespace MvcApplicationWithVersioning.Reflection
{
    public class TypesFinder
    {
        public static Type FindTypeInExecutingAssembly(string typeName)
        {
            var currentAssembly = System.Reflection.Assembly.GetExecutingAssembly();

            // todo: add caching of found types
            var type = currentAssembly.GetTypes().FirstOrDefault(t => string.CompareOrdinal(t.Name, typeName) == 0);

            return type;
        }
    }
}