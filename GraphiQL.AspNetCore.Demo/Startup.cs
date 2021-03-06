using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GraphiQL.AspNetCore.Demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseGraphiQL(new GraphiQLSettings
            {
                DefaultQuery = "{defaultQuery(value: \"1\")}",
                Query = "{defaultQuery(value: \"1\")}",
                GraphiQLPath = "graphql",
                GraphQLEndpoint = "graphql-endpoint",
                PageTitle = "some-title"
            });

            app.UseGraphiQL(build => build
                    .DefaultQuery("{defaultQuery(value: \"2\")}")
                    .Query("{defaultQuery(value: \"2\")}")
                    .GraphiQLPath("graphql2")
                    .GraphQLEndpoint("graphql-endpoint2")
                    .PageTitle("some-title-2")
            );

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
