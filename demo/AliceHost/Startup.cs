using System.Collections.Generic;
using Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Zaaby.Client;
using Zaaby.Core;

namespace AliceHost
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddZaaby<IService>();
            services.UseZaabyClient(new Dictionary<string, List<string>>
            {
                {"IBobServices", new List<string> {"http://localhost:5002"}},
                {"ICarolServices", new List<string> {"http://localhost:5003"}}
            });
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("Alice", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Alice API",
                    Description = "API for AliceHost",
                    Contact = new OpenApiContact {Name = "DuXiaoFei", Email = "aeondxf@live.com"}
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseZaaby();
            //Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            //Enable middleware to serve swagger-ui (HTML, JS, CSS etc.), specifying the Swagger JSON endpoint
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/Alice/swagger.json", "Alice Docs");

                option.RoutePrefix = string.Empty;
                option.DocumentTitle = "Alice API";
            });
        }
    }
}