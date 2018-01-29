using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GraphiQL.AspNetCore
{
    public class GraphiQLConfigurationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly GraphiQLSettings _graphiQLSettings;

        public GraphiQLConfigurationMiddleware(RequestDelegate next, GraphiQLSettings graphiQLSettings)
        {
            _graphiQLSettings = graphiQLSettings;
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (IsPathMatch(context.Request.Path))
            {
                var fields = new [] {
                    Map("graphQLEndpoint", _graphiQLSettings.GraphQLEndpoint),
                    Map("defaultQuery", System.Web.HttpUtility.JavaScriptStringEncode(_graphiQLSettings.DefaultQuery)),
                    Map("query", System.Web.HttpUtility.JavaScriptStringEncode(_graphiQLSettings.Query))
                };

                await context.Response.WriteAsync("var graphiQLConfig = { " + string.Join(", ", fields) + " };");
            }
            else
            {
                await _next.Invoke(context);
            }
        }

        private static string Map(string key, string value)
        {
            return $"{key}: \"{value}\"";
        }

        private static string Map(string key, bool value)
        {
            return $"{key}: {value.ToString().ToLowerInvariant()}";
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
                ? $"{_graphiQLSettings.GraphiQLPath}/config"
                : $"/{_graphiQLSettings.GraphiQLPath}/config";
        }
    }
}