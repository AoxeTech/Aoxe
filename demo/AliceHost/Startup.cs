using System.Collections.Generic;
using Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
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
            services.AddZaaby<ITest>();
            services.UseZaabyClient(new Dictionary<string, List<string>>
            {
                {"IBobServices", new List<string> {"http://localhost:5002"}},
                {"ICarolServices", new List<string> {"http://localhost:5003"}}
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseZaaby();
        }
    }
}