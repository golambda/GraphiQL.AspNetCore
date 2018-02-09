using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace GraphiQL.AspNetCore.Test
{
    public class GraphiQLMiddlewareTest
    {
        private const string GraphiQLPath = "some-graphql-endpoint";
        private const string FaviconPath = "some-favicon.ico";
        private const string PageTitle = "some-title";

        [Theory]
        [InlineData("/" + GraphiQLPath + "/")]
        [InlineData("/" + GraphiQLPath)]
        public async Task WritesHtmlToResponseWhenGraphiQLPath(string path)
        {
            var httpContext = HttpContextMother.Create(path);
            var graphiQLMiddleware = CreateGraphiQLMiddleware();
            await graphiQLMiddleware.Invoke(httpContext);

            httpContext.Response.Body.Position = 0;

            using (var reader = new StreamReader(httpContext.Response.Body))
            {
                var text = reader.ReadToEnd();
                Assert.Equal(GetIndeHtml(), text);
            }
        }

        [Theory]
        [InlineData("/" + GraphiQLPath + "/", "application/json; charset=utf-8")]
        [InlineData("/" + GraphiQLPath + "/", "application/json")]
        [InlineData("/" + GraphiQLPath + "/", "charset=utf-8; application/json")]
        [InlineData("/" + GraphiQLPath, "application/json; charset=utf-8")]
        [InlineData("/" + GraphiQLPath, "application/json")]
        [InlineData("/" + GraphiQLPath, "charset=utf-8; application/json")]
        public async Task AllowsRequestToBypassIfApplicationJson(string path, string contentType)
        {
            var httpContext = HttpContextMother.Create(path);
            httpContext.Request.ContentType = contentType;
            var graphiQLMiddleware = CreateGraphiQLMiddleware();
            await graphiQLMiddleware.Invoke(httpContext);

            httpContext.Response.Body.Position = 0;

            using (var reader = new StreamReader(httpContext.Response.Body))
            {
                var text = reader.ReadToEnd();
                Assert.Equal(string.Empty, text);
            }
        }

        [Fact]
        public async Task AllowsTheRequestToContinueIfFileIsUnknown()
        {
            var httpContext = HttpContextMother.Create("/" + GraphiQLPath + "/foo.js");
            var graphiQLMiddleware = CreateGraphiQLMiddleware();
            await graphiQLMiddleware.Invoke(httpContext);

            Assert.Equal(42, httpContext.Response.StatusCode);
        }

        private static GraphiQLMiddleware CreateGraphiQLMiddleware()
        {
            async Task RequestDelegate(HttpContext httpContent)
            {
                httpContent.Response.StatusCode = 42;
                await Task.FromResult(0);
            }

            return new GraphiQLMiddleware(RequestDelegate, CreateGraphiQLSettings(), new GraphiQLRouteChecker(GraphiQLPath));
        }

        private static GraphiQLSettings CreateGraphiQLSettings()
        {
            return new GraphiQLSettings
            {
                GraphQLEndpoint = "http://some-endpoint.com",
                GraphiQLPath = GraphiQLPath,
                PageTitle = PageTitle,
                FaviconPath = FaviconPath
            };
        }

        private string GetIndeHtml()
        {
            using (var stream = Assembly.GetAssembly(typeof(GraphiQLMiddleware))
                .GetManifestResourceStream("GraphiQL.AspNetCore.assets.index.html"))
            {
                using (var reader = new StreamReader(stream))
                {
                    return reader
                        .ReadToEnd()
                        .Replace("{{baseUrl}}", "/" + GraphiQLPath)
                        .Replace("{{favicon}}", GetFaviconHtml(FaviconPath))
                        .Replace("{{title}}", PageTitle);
                }
            }
        }

        private string GetFaviconHtml(string faviconPath)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(
                $"        <link rel=\"shortcut icon\" href=\"{faviconPath}\" type=\"image/x-icon\">");
            stringBuilder.AppendLine(
                $"        <link rel=\"icon\" href=\"{faviconPath}\" type = \"image/x-icon\">");
            return stringBuilder.ToString();
        }
    }
}