using System.Reflection;

namespace GraphiQL.AspNetCore
{
    public class GraphiQLConfigurationBuilder
    {
        private readonly GraphiQLSettings _graphiQLSettings = new GraphiQLSettings();

        public GraphiQLConfigurationBuilder GraphiQLPath(string graphiQLPath)
        {
            _graphiQLSettings.GraphiQLPath = graphiQLPath;
            return this;
        }
        public GraphiQLConfigurationBuilder DefaultQuery(string defaultQuery)
        {
            _graphiQLSettings.DefaultQuery = defaultQuery;
            return this;
        }
        public GraphiQLConfigurationBuilder GraphQLEndpoint(string graphQLEndpoint)
        {
            _graphiQLSettings.GraphQLEndpoint = graphQLEndpoint;
            return this;
        }
        public GraphiQLConfigurationBuilder PageTitle(string pageTitle)
        {
            _graphiQLSettings.PageTitle = pageTitle;
            return this;
        }
        public GraphiQLConfigurationBuilder Query(string query)
        {
            _graphiQLSettings.Query = query;
            return this;
        }
        public GraphiQLConfiguration Build()
        {
            var graphiQLResourceLoader = new GraphiQLResourceLoader(Assembly.GetAssembly(typeof(GraphiQLMiddleware)));
            var graphiQLRouteChecker = new GraphiQLRouteChecker(_graphiQLSettings.GraphiQLPath);
            return new GraphiQLConfiguration(_graphiQLSettings, graphiQLRouteChecker, graphiQLResourceLoader);
        }
    }
}