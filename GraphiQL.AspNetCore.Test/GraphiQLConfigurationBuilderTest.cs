using Xunit;

namespace GraphiQL.AspNetCore.Test
{
    public class GraphiQLConfigurationBuilderTest
    {
        private const string GraphiQLPath = "/some-path";
        private const string DefaultQuery = "default-query";
        private const string GraphQLEndpoint = "some-endpoint";
        private const string PageTitle = "page-title";
        private const string Query = "some-query";

        [Fact]
        public void BuildsGraphiQLConfiguration()
        {
            var builder = new GraphiQLConfigurationBuilder();
            var configuration = builder
                .GraphiQLPath(GraphiQLPath)
                .DefaultQuery(DefaultQuery)
                .GraphQLEndpoint(GraphQLEndpoint)
                .PageTitle(PageTitle)
                .Query(Query)
                .Build();

            Assert.Equal(GraphiQLPath, configuration.Settings.GraphiQLPath);
            Assert.Equal(DefaultQuery, configuration.Settings.DefaultQuery);
            Assert.Equal(GraphQLEndpoint, configuration.Settings.GraphQLEndpoint);
            Assert.Equal(PageTitle, configuration.Settings.PageTitle);
            Assert.Equal(Query, configuration.Settings.Query);
            Assert.NotNull(configuration.RouteChecker);
            Assert.IsType<GraphiQLRouteChecker>(configuration.RouteChecker);
            Assert.NotNull(configuration.ResourceLoader);
            Assert.IsType<GraphiQLResourceLoader>(configuration.ResourceLoader);
        }
    }
}