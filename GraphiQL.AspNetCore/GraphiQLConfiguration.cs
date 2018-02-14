using System;

namespace GraphiQL.AspNetCore
{
    public class GraphiQLConfiguration
    {
        public GraphiQLSettings Settings { get; }
        public IGraphiQLRouteChecker RouteChecker { get; }
        public IGraphiQLResourceLoader ResourceLoader { get; }
        public GraphiQLConfiguration(
            GraphiQLSettings settings,
            IGraphiQLRouteChecker routeChecker,
            IGraphiQLResourceLoader resourceLoader)
        {
            Settings = settings;
            RouteChecker = routeChecker;
            ResourceLoader = resourceLoader;
        }
    }
}