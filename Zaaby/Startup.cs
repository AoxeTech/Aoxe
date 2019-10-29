using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Zaaby
{
    internal class Startup
    {
        internal static readonly List<ServiceDescriptor> ServiceDescriptors = new List<ServiceDescriptor>();

        //Key is interface type,value is implement type
        internal static readonly Dictionary<Type, Type> ServiceDic = new Dictionary<Type, Type>();
        internal static readonly List<Type> ServiceRunnerTypes = new List<Type>();

        public void ConfigureServices(IServiceCollection services)
        {
            services.Add(ServiceDescriptors);
            foreach (var (interfaceType, implementType) in ServiceDic)
                services.AddScoped(interfaceType, implementType);

            services.AddControllers(options =>
                {
                    foreach (var (interfaceType, implementType) in ServiceDic)
                        options.Conventions.Add(new ZaabyActionModelConvention(interfaceType));
                }).ConfigureApplicationPartManager(manager =>
                {
                    manager.FeatureProviders.Add(new ZaabyAppServiceControllerFeatureProvider(ServiceDic.Values));
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
                });
        }

        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            app.UseErrorHandling();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            ServiceRunnerTypes.ForEach(type => serviceProvider.GetService(type));
        }
    }
}