using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Zaaby
{
    internal class Startup
    {
        internal static readonly Dictionary<Type, Type> ScopeDic = new Dictionary<Type, Type>();
        internal static readonly Dictionary<Type, Type> TransientDic = new Dictionary<Type, Type>();
        internal static readonly Dictionary<Type, Type> SingletonDic = new Dictionary<Type, Type>();
        internal static readonly List<ServiceDescriptor> ServiceDescriptors = new List<ServiceDescriptor>();

        //Key is interface type,value is implement type
        internal static readonly Dictionary<Type, Type> ServiceDic = new Dictionary<Type, Type>();
        internal static readonly List<Type> ServiceRunnerTypes = new List<Type>();

        public void ConfigureServices(IServiceCollection services)
        {
            foreach (var keyValuePair in TransientDic)
                services.AddTransient(keyValuePair.Key, keyValuePair.Value);

            foreach (var keyValuePair in ScopeDic)
                services.AddScoped(keyValuePair.Key, keyValuePair.Value);

            foreach (var keyValuePair in SingletonDic)
                services.AddSingleton(keyValuePair.Key, keyValuePair.Value);

            services.Add(ServiceDescriptors);

            var serviceProvider = services.BuildServiceProvider();

            ServiceRunnerTypes.ForEach(type => serviceProvider.GetService(type));

            services.AddMvcCore(mvcOptions =>
                {
                    foreach (var keyValuePair in ServiceDic)
                    {
                        var interfaceType = keyValuePair.Key;
                        var implementType = keyValuePair.Value;
                        services.AddScoped(interfaceType, implementType);
                        mvcOptions.Conventions.Add(new ZaabyActionModelConvention(interfaceType));
                    }
                    mvcOptions.Filters.Add(typeof(WebApiResultFilter));
                })
                .ConfigureApplicationPartManager(manager =>
                {
                    manager.FeatureProviders
                        .Add(new ZaabyAppServiceControllerFeatureProvider(ServiceDic.Values));
                }).AddJsonFormatters();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseErrorHandling();
            app.UseMvc();
        }
    }
}