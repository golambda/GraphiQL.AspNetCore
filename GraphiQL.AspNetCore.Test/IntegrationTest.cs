using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Xunit;

namespace GraphiQL.AspNetCore.Test
{
    public class IntegrationTest : IDisposable
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;

        public IntegrationTest()
        {
            _server = new TestServer(new WebHostBuilder()
                .Configure(x => x.UseGraphiQL()));
            _client = _server.CreateClient();
        }

        [Fact]
        public async Task GraphQLEndpointReturnsGraphiQLPage()
        {
            var response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "/graphql"));
            var body = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("<html>", body);
        }

        [Theory]
        [InlineData("/graphql/bundle.js")]
        [InlineData("/graphql/style.css")]
        [InlineData("/graphql/graphiql.css")]
        [InlineData("/graphql/config")]
        public async Task GraphQLEndpointReturnsAsset(string url)
        {
            var response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
            var body = await response.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotEmpty(body);
        }

        [Fact]
        public async Task UnknownGraphQLEndpointReturnsNotFound()
        {
            var response = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "/graphql/not-known"));
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        public void Dispose()
        {
            _server?.Dispose();
            _client?.Dispose();
        }
    }
}