using System.IO;

namespace GraphiQL.AspNetCore
{
    public interface IGraphiQLResourceLoader
    {
        Stream Load(string filePath);
    }
}