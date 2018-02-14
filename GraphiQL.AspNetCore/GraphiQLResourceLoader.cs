using System.IO;
using System.Reflection;

namespace GraphiQL.AspNetCore
{
    public class GraphiQLResourceLoader : IGraphiQLResourceLoader
    {
        private readonly Assembly _assembly;

        public GraphiQLResourceLoader(Assembly assembly)
        {
            _assembly = assembly;
        }

        public Stream Load(string filePath)
        {
            var resourceName = $"GraphiQL.AspNetCore.assets.{filePath}";
            return _assembly.GetManifestResourceStream(resourceName);
        }
    }
}