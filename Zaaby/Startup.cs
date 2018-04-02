using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Zaaby
{
    internal class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var types = GetTypes();

            var serviceInterfaces = types.Where(type => type.IsInterface && type.Namespace == "IServices").ToList();

            var implementServices = types
                .Where(type => type.IsClass && serviceInterfaces.Any(i => i.IsAssignableFrom(type)))
                .ToList();

            implementServices.ForEach(service =>
            {
                var serviceInterface = service.GetInterfaces().FirstOrDefault(i => serviceInterfaces.Contains(i));
                if (serviceInterface != null)
                    services.AddScoped(serviceInterface, service);
            });

            services.AddMvcCore(mvcOptions =>
                {
                    serviceInterfaces.ForEach(type =>
                    {
                        mvcOptions.Conventions.Add(new ActionModelConvention(type));
                    });
                })
                .ConfigureApplicationPartManager(manager =>
                {
                    manager.FeatureProviders.Add(new ZaabyAppServiceControllerFeatureProvider(implementServices));
                }).AddJsonFormatters();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseMvc();
        }

        private static List<Type> GetTypes()
        {
            var dir = Directory.GetCurrentDirectory();
            var files = new List<string>();

            files.AddRange(Directory.GetFiles(dir + @"\", "*.dll", SearchOption.AllDirectories));
            files.AddRange(Directory.GetFiles(dir + @"\", "*.exe", SearchOption.AllDirectories));

            var typeDic = new Dictionary<string, Type>();

            foreach (var file in files)
            {
                try
                {
                    foreach (var type in Assembly.LoadFrom(file).GetTypes())
                        if (!typeDic.ContainsKey(type.FullName))
                            typeDic.Add(type.FullName, type);
                }
                catch (BadImageFormatException)
                {
                    // ignored
                }
            }

            return typeDic.Select(kv => kv.Value).ToList();
        }
    }
}