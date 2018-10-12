using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Zaabee.AspNetCore.Formatters.Protobuf;

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

            services.AddMvcCore(options =>
                {
                    foreach (var keyValuePair in ServiceDic)
                    {
                        var interfaceType = keyValuePair.Key;
                        var implementType = keyValuePair.Value;
                        services.AddScoped(interfaceType, implementType);
                        options.Conventions.Add(new ZaabyActionModelConvention(interfaceType));
                    }
                    options.Filters.Add(typeof(WebApiResultFilter));
                    options.AddProtobufFormatter();
                })
                .ConfigureApplicationPartManager(manager =>
                {
                    manager.FeatureProviders
                        .Add(new ZaabyAppServiceControllerFeatureProvider(ServiceDic.Values));
                }).AddJsonFormatters();
        }

        public void Configure(IApplicationBuilder app, IServiceProvider serviceProvider)
        {
            app.UseErrorHandling();
            app.UseMvc();
            ServiceRunnerTypes.ForEach(type => serviceProvider.GetService(type));
        }
    }
}