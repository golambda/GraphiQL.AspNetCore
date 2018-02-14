using System;
using System.Reflection;
using Microsoft.AspNetCore.Builder;

namespace GraphiQL.AspNetCore
{
    public static class GraphiQLExtensions
    {
        public static IApplicationBuilder UseGraphiQL(this IApplicationBuilder app)
        {
            return app.UseGraphiQL(new GraphiQLSettings());
        }

        public static IApplicationBuilder UseGraphiQL(this IApplicationBuilder app, GraphiQLSettings graphiQLSettings)
        {
            return app.UseGraphiQL(new GraphiQLConfiguration(
                    graphiQLSettings,
                    new GraphiQLRouteChecker(graphiQLSettings.GraphiQLPath),
                    new GraphiQLResourceLoader(Assembly.GetAssembly(typeof(GraphiQLMiddleware)))));
        }

        public static IApplicationBuilder UseGraphiQL(this IApplicationBuilder app, Action<GraphiQLConfigurationBuilder> setUp)
        {
            var builder = new GraphiQLConfigurationBuilder();
            setUp(builder);
            return app.UseGraphiQL(builder.Build());
        }

        private static IApplicationBuilder UseGraphiQL(this IApplicationBuilder app, GraphiQLConfiguration graphiQLConfiguration)
        {
            return app
                .UseMiddleware<GraphiQLConfigurationMiddleware>(graphiQLConfiguration.Settings)
                .UseMiddleware<GraphiQLMiddleware>(graphiQLConfiguration);
        }
    }
}