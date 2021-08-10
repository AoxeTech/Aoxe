using System.Collections.Generic;
using AliceServices;
using IAliceServices;
using IBobServices;
using ICarolServices;
using Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Zaaby.Client.Http;
using Zaaby.Common;
using Zaaby.Server;

namespace AliceHost
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.FromAssemblyOf<IAliceService>()
                .FromAssemblyOf<IBobService>()
                .FromAssemblyOf<ICarolService>()
                .FromAssemblyOf<AliceService>()
                .AddZaabyService<IService>()
                .AddZaabyService<ServiceAttribute>()
                .AddZaabyClient(typeof(IService),new Dictionary<string, string>
                {
                    {"IBobServices", "http://localhost:5002"},
                    {"ICarolServices", "http://localhost:5003"}
                });
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
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
            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();    
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Alice API v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}