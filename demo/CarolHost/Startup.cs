using System.Collections.Generic;
using CarolServices;
using IAliceServices;
using IBobServices;
using ICarolServices;
using Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zaaby.Client;
using Zaaby.Common;
using Zaaby.Service;

namespace CarolHost
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.FromAssemblyOf(typeof(IAliceService), typeof(IBobService), typeof(ICarolService),
                typeof(CarolService));
            services.AddZaabyService<IService>();
            services.UseZaabyClient(typeof(IService),new Dictionary<string, List<string>>
            {
                {"IAliceServices", new List<string> {"https://localhost:5001"}},
                {"IBobServices", new List<string> {"http://localhost:5002"}}
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseZaabyErrorHandling();
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}