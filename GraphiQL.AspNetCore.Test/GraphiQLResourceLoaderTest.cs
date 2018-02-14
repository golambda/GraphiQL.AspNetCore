using System.Reflection;
using Xunit;

namespace GraphiQL.AspNetCore.Test
{
    public class GraphiQLResourceLoaderTest
    {
        [Theory]
        [InlineData("bundle.js")]
        [InlineData("style.css")]
        [InlineData("graphiql.css")]
        public void GraphiQLResourceLoaderReturnsStream(string fileName)
        {
            var graphiQLResourceLoader = new GraphiQLResourceLoader(Assembly.GetAssembly(typeof(GraphiQLMiddleware)));

            var stream = graphiQLResourceLoader.Load(fileName);

            Assert.NotNull(stream);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("not-found")]
        public void GraphiQLResourceLoaderReturnsNull(string fileName)
        {
            var graphiQLResourceLoader = new GraphiQLResourceLoader(Assembly.GetAssembly(typeof(GraphiQLMiddleware)));

            var stream = graphiQLResourceLoader.Load(fileName);

            Assert.Null(stream);
        }       
    }
}