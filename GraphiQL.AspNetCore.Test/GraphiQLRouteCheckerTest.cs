using Xunit;

namespace GraphiQL.AspNetCore.Test
{
    public class GraphiQLRouteCheckerTest
    {
        
        private const string GraphiQLPath = "some-graphql-endpoint";
        private const string NotGraphiQLPath = "not-graphql-endpoint";

        [Theory]
        [InlineData("/" + GraphiQLPath + "/", null, true)]
        [InlineData("/" + GraphiQLPath + "/", "text/html", true)]
        [InlineData("/" + NotGraphiQLPath + "/", null, false)]
        [InlineData("/" + NotGraphiQLPath + "/", "application/json; charset=utf-8", false)]
        [InlineData("/" + GraphiQLPath + "/", "application/json; charset=utf-8", false)]
        [InlineData("/" + GraphiQLPath + "/", "application/json", false)]
        [InlineData("/" + GraphiQLPath + "/", "charset=utf-8; application/json", false)]
        [InlineData("/" + GraphiQLPath, "application/json; charset=utf-8", false)]
        [InlineData("/" + GraphiQLPath, "application/json", false)]
        [InlineData("/" + GraphiQLPath, "charset=utf-8; application/json", false)]
        public void ChecksRouteAndContentType(string path, string contentType, bool expectedIsMatch)
        {
            var httpContext = HttpContextMother.Create(path);
            httpContext.Request.ContentType = contentType;

            var graphiQLRouteChecker = new GraphiQLRouteChecker(GraphiQLPath);
            var actualIsMatch = graphiQLRouteChecker.IsMatch(httpContext);

            Assert.Equal(expectedIsMatch, actualIsMatch);
        }
    }
}