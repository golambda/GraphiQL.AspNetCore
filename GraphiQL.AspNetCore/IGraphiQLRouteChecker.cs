using Microsoft.AspNetCore.Http;

namespace GraphiQL.AspNetCore
{
    public interface IGraphiQLRouteChecker
    {
        bool IsMatch(HttpContext httpContext);
    }
}