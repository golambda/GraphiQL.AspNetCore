using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

namespace GraphiQL.AspNetCore.Test
{
    public static class HttpContextMother
    {
        public static HttpContext Create(string path)
        {
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Path = path;
            httpContext.Request.EnableRewind();
            httpContext.Response.Body = new MemoryStream();
            return httpContext;
        }
    }
}
