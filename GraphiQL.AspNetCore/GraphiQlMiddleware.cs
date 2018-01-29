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

        public GraphiQLMiddleware(RequestDelegate next, GraphiQLSettings graphiQLSettings)
        {
            _graphiQLSettings = graphiQLSettings;
            _next = next;
            _assembly = typeof(GraphiQLExtensions).GetTypeInfo().Assembly;
        }

        public async Task Invoke(HttpContext context)
        {
            if (IsPathMatch(context.Request.Path) && IsCorrectContentType(context))
            {
                var fileName = GetFileName(context);

                var resourceName = $"GraphiQL.AspNetCore.assets.{fileName}";

                using (var stream = _assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream == null)
                    {
                        await _next.Invoke(context);
                    }
                    else
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            var content = reader.ReadToEnd();

                            if (resourceName.EndsWith(".html"))
                            {
                                await context.Response.WriteAsync(BuildHtml(content));
                            }
                            else
                            {
                                await context.Response.WriteAsync(content);
                            }
                        }
                    }
                }
            }
            else
            {
                await _next.Invoke(context);
            }
        }

        private bool IsPathMatch(PathString requestPath)
        {
            return requestPath != null &&
                   requestPath.HasValue &&
                   requestPath.Value.ToLowerInvariant().StartsWith(GetGraphiQLPathConfig());
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

        private static bool IsCorrectContentType(HttpContext httpContext)
        {
            return 
                string.IsNullOrEmpty(httpContext.Request.ContentType) ||
                !httpContext.Request.ContentType.Contains("application/json");
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