using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Xunit;

namespace GraphiQL.AspNetCore.Test
{
    public class GraphiQLConfigurationMiddlewareTest
    {
        private const string GraphiQLPath = "some-graphql-endpoint";

        [Fact]
        public async Task WriteConfigToResponseWhenGraphiQLConfigPath()
        {
            var graphiQLSettings = new GraphiQLSettings
            {
                GraphQLEndpoint = "http://some-endpoint.com",
            };

            var text = await GetResponse("/graphql/config", graphiQLSettings);
            Assert.StartsWith("var graphiQLConfig = {", text);
            Assert.EndsWith("};", text);
        }

        [Fact]
        public async Task MapCorrectFieldsToJavascriptObject()
        {
            var graphiQLSettings = new GraphiQLSettings
            {
                GraphiQLPath = GraphiQLPath,
                GraphQLEndpoint = "http://some-endpoint.com",
                DefaultQuery = "some-default-query",
                Query = "some-query"
            };

            var jObject = await GetReponseAsJObject(graphiQLSettings);

            AssertMapping(graphiQLSettings.GraphQLEndpoint, jObject, "graphQLEndpoint");
            AssertMapping(graphiQLSettings.DefaultQuery, jObject, "defaultQuery");
            AssertMapping(graphiQLSettings.Query, jObject, "query");
        }

        [Fact]
        public async Task DoesNotWriteConfigToResponseWhenNotGraphiQLConfigPath()
        {
            var graphiQLSettings = new GraphiQLSettings
            {
                GraphQLEndpoint = "http://some-endpoint.com"
            };

            var text = await GetResponse("/other-path", graphiQLSettings);

            Assert.Equal(string.Empty, text);
        }

        private static void AssertMapping(string expectedValue, JObject jObject, string fieldPath)
        {
            try
            {
                Assert.Equal(expectedValue, jObject[fieldPath].ToString());
            }
            catch
            {
               throw new Exception($"{fieldPath} has failed to map correctly"); 
            }
        }

        private static async Task<string> GetResponse(string path, GraphiQLSettings graphiQLSettings)
        {
            var httpContext = HttpContextMother.Create(path);
            var graphiQLConfigurationMiddleware =
                new GraphiQLConfigurationMiddleware(async _ => await Task.FromResult(0), graphiQLSettings);
            await graphiQLConfigurationMiddleware.Invoke(httpContext);

            httpContext.Response.Body.Position = 0;

            using (var reader = new StreamReader(httpContext.Response.Body))
            {
                return reader.ReadToEnd();
            }
        }

        private static async Task<JObject> GetReponseAsJObject(GraphiQLSettings graphiQLSettings)
        {
            var text = await GetResponse($"/{graphiQLSettings.GraphiQLPath}/config", graphiQLSettings);
            var json = text
                .Replace("var graphiQLConfig = ", string.Empty)
                .Replace(";", string.Empty);

            var jObject = JObject.Parse(json);
            return jObject;
        }
    }
}
