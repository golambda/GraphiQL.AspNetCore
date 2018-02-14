using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GraphiQL.AspNetCore
{
    public class GraphiQLMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly GraphiQLSettings _graphiQLSettings;
        private readonly Assembly _assembly;
        private readonly IGraphiQLRouteChecker _graphiQLRouteChecker;
        private readonly IGraphiQLResourceLoader _graphiQLResourceLoader;

        public GraphiQLMiddleware(
            RequestDelegate next,
            GraphiQLConfiguration graphiQLConfiguration)
        {
            _graphiQLResourceLoader = graphiQLConfiguration.ResourceLoader;
            _graphiQLRouteChecker = graphiQLConfiguration.RouteChecker;
            _graphiQLSettings = graphiQLConfiguration.Settings;
            _next = next;
            _assembly = typeof(GraphiQLExtensions).GetTypeInfo().Assembly;
        }

        public async Task Invoke(HttpContext context)
        {
            if (_graphiQLRouteChecker.IsMatch(context))
            {
                var fileName = GetFileName(context);

                using (var stream = _graphiQLResourceLoader.Load(fileName))
                {
                    if (stream == null)
                    {
                        await _next.Invoke(context);
                    }
                    else
                    {
                        if (fileName.EndsWith(".html"))
                        {
                            using (var reader = new StreamReader(stream))
                            {
                                var content = reader.ReadToEnd();
                                await context.Response.WriteAsync(BuildHtml(content));
                            }
                        }
                        else
                        {
                            await stream.CopyToAsync(context.Response.Body);
                        }

                    }
                }
            }
            else
            {
                await _next.Invoke(context);
            }
        }

        private string GetGraphiQLPathConfig()
        {
            return _graphiQLSettings.GraphiQLPath.StartsWith("/")
                ? _graphiQLSettings.GraphiQLPath
                : $"/{_graphiQLSettings.GraphiQLPath}";
        }

        private string GetFileName(HttpContext context)
        {
            var basePathLength = GetGraphiQLPathConfig().Length;
            if (context.Request.Path.Value.Length < basePathLength)
            {
                return "index.html";
            }

            var fileName = context.Request.Path.Value
                .Substring(basePathLength, context.Request.Path.Value.Length - basePathLength)
                .Replace("/", string.Empty);

            return string.IsNullOrEmpty(fileName)
                ? "index.html"
                : fileName;
        }

        private string BuildHtml(string rawHtml)
        {
            return rawHtml
                .Replace("{{baseUrl}}", _graphiQLSettings.GraphiQLPath.StartsWith("/") ? _graphiQLSettings.GraphiQLPath : "/" + _graphiQLSettings.GraphiQLPath)
                .Replace("{{title}}", _graphiQLSettings.PageTitle)
                .Replace("{{favicon}}",
                    string.IsNullOrEmpty(_graphiQLSettings.FaviconPath) ? string.Empty : GetFaviconHtml());
        }

        private string GetFaviconHtml()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(
                $"        <link rel=\"shortcut icon\" href=\"{_graphiQLSettings.FaviconPath}\" type=\"image/x-icon\">");
            stringBuilder.AppendLine(
                $"        <link rel=\"icon\" href=\"{_graphiQLSettings.FaviconPath}\" type = \"image/x-icon\">");
            return stringBuilder.ToString();
        }
    }
}