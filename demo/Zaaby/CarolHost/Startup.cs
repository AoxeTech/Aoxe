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
using Zaaby.AspNetCore.Formatters.Jil;
using Zaaby.AspNetCore.Formatters.MsgPack;
using Zaaby.AspNetCore.Formatters.Protobuf;
using Zaaby.AspNetCore.Formatters.Utf8Json;
using Zaaby.AspNetCore.Formatters.ZeroFormatter;
using Zaaby.Client.Http;
using Zaaby.Client.Http.Formatter.Jil;
using Zaaby.Shared;
using Zaaby.Server;

namespace CarolHost
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJil()
                .AddMsgPack()
                .AddProtobuf()
                .AddUtf8Json()
                .AddZeroFormatter()
                .AddXmlSerializerFormatters();
            services.FromAssemblies(typeof(IAliceService).Assembly, typeof(IBobService).Assembly);
            services.FromAssemblyNames(typeof(ICarolService).Assembly.GetName(),
                typeof(CarolService).Assembly.GetName());
            services.AddZaabyService<IService>();
            services.AddZaabyClient(typeof(IService), new Dictionary<string, string>
            {
                {"IAliceServices", "http://localhost:5001"},
                {"IBobServices", "http://localhost:5002"}
            }, options => options.UseJilFormatter());
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