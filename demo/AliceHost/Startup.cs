using System.Collections.Generic;
using Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Zaaby.Client;
using Zaaby.Service;

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