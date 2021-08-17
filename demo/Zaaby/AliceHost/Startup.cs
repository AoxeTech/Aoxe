using System;
using System.Collections.Generic;
using AliceServices;
using Consul;
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
using Zaaby.Consul;
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
            services.AddConsul(options =>
            {
                options.ConsulAddress = "http://192.168.78.140:8500/";
                options.AgentServiceRegistration = new AgentServiceRegistration
                {
                    ID = Guid.NewGuid().ToString(),
                    Name = GetType().Namespace,
                    Address = "https://172.16.20.26",
                    Port = 5001,
                    Tags = new[] { "api" },
                    Check = new AgentServiceCheck
                    {
                        DeregisterCriticalServiceAfter = TimeSpan.FromMinutes(1),
                        Interval = TimeSpan.FromSeconds(30),
                        HTTP = "https://172.16.20.26:5001/HealthCheck"
                    }
                };
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