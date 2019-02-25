using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AwesomeSauceApi
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            /* Enpoints
            app.Use(async (context, next) =>
            {
                if(context.Request.Path == "/foo")
                {
                    await context.Response.WriteAsync($"Welcome to Foo");
                }
                else
                {
                    await next();
                }
            });

            app.Use(async (context, next) =>
            {
                if (context.Request.Path == "/bar")
                {
                    await context.Response.WriteAsync($"Welcome to Bar");
                }
                else
                {
                    await next();
                }
            }); 
            app.Run(async (context) => await context.Response.WriteAsync($"Welcome to the default"));
            */

            /*Enppoints using Map and MapWhen */

            app.Map("/foo", config => config.Use(async (context, next) => await context.Response.WriteAsync("Welcome to /foo")));

            app.MapWhen(context => context.Request.Method == "POST" && context.Request.Path == "/bar",
                config => config.Use(async (context, next) => await context.Response.WriteAsync("Welcome to POST /bar")));

            app.Run(async (context) => await context.Response.WriteAsync($"Welcome to the default"));
        }
    }
}
