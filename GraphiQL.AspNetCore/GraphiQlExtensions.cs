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
            return app
                .UseMiddleware<GraphiQLConfigurationMiddleware>(graphiQLSettings)
                .UseMiddleware<GraphiQLMiddleware>(graphiQLSettings);
        }
    }
}