using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Zaaby.Core.Application;
using Zaaby.Core.Domain;

namespace Zaaby
{
    internal class Startup
    {
        internal static readonly Dictionary<Type, Type> ScopeDic = new Dictionary<Type, Type>();
        internal static readonly Dictionary<Type, Type> TransientDic = new Dictionary<Type, Type>();
        internal static readonly Dictionary<Type, Type> SingletonDic = new Dictionary<Type, Type>();
        internal static readonly List<ServiceDescriptor> ServiceDescriptors = new List<ServiceDescriptor>();

        internal static readonly List<Action<MvcOptions>> AddMvcCoreActions = new List<Action<MvcOptions>>();

        internal static readonly List<Action<ApplicationPartManager>> ConfigureApplicationPartManagerActions =
            new List<Action<ApplicationPartManager>>();

        internal static readonly List<Type> DomainEventHandlerTypes = new List<Type>();
        internal static readonly List<Type> IntegrationEventHandlerTypes = new List<Type>();

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
            
            DomainEventHandlerTypes.ForEach(type => serviceProvider.GetService(type));
            
            IntegrationEventHandlerTypes.ForEach(type => serviceProvider.GetService(type));

            services.AddMvcCore(mvcOptions => { AddMvcCoreActions.ForEach(action => action.Invoke(mvcOptions)); })
                .ConfigureApplicationPartManager(manager =>
                {
                    ConfigureApplicationPartManagerActions.ForEach(action => action.Invoke(manager));
                }).AddJsonFormatters();
        }

        public void Configure(IApplicationBuilder app)
        {
            //domainEventHandleRunner?.Run();
            //integrationEventHandlerRunner?.Run();
            app.UseMvc();
        }
    }
}