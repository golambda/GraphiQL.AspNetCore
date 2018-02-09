using Microsoft.AspNetCore.Http;

namespace GraphiQL.AspNetCore
{
    public class GraphiQLRouteChecker : IGraphiQLRouteChecker
    {
        private readonly string _path;

        public GraphiQLRouteChecker(string path)
        {
            _path = path;
        }

        public bool IsMatch(HttpContext httpContext)
        {
            return IsPathMatch(httpContext.Request.Path) && IsCorrectContentType(httpContext);
        }

        private bool IsPathMatch(PathString requestPath)
        {
            return requestPath != null &&
                   requestPath.HasValue &&
                   requestPath.Value.ToLowerInvariant().StartsWith(FormattedPath());
        }

        private static bool IsCorrectContentType(HttpContext httpContext)
        {
            return
                string.IsNullOrEmpty(httpContext.Request.ContentType) ||
                !httpContext.Request.ContentType.Contains("application/json");
        }

        private string FormattedPath()
        {
            return _path.StartsWith("/")
                ? _path
                : $"/{_path}";
        }
    }
}