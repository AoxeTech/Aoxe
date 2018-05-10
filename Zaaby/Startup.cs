using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Zaaby.Core.Domain;
using Zaaby.Core.Infrastructure.EventBus;

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

//        internal static readonly List<IDomainEventHandler<,>> lll = new List<>();

        private void ConfigureServices(IServiceCollection services, IEventBus eventBus)
        {
            foreach (var keyValuePair in TransientDic)
                services.AddTransient(keyValuePair.Key, keyValuePair.Value);

            foreach (var keyValuePair in ScopeDic)
                services.AddScoped(keyValuePair.Key, keyValuePair.Value);

            foreach (var keyValuePair in SingletonDic)
                services.AddSingleton(keyValuePair.Key, keyValuePair.Value);

            services.Add(ServiceDescriptors);

            var i = services.BuildServiceProvider().GetServices(typeof(IDomainEventHandler<,>));
            foreach (var o in i)
            {                                                                                                                                                                                                                                                        
                var oo = o as IDomainEventHandler<IDomainEvent<Guid>, Guid>;
                eventBus.SubscribeEvent<IDomainEvent<Guid>>(oo.Handle);
            }

            services.AddMvcCore(mvcOptions => { AddMvcCoreActions.ForEach(action => action.Invoke(mvcOptions)); })
                .ConfigureApplicationPartManager(manager =>
                {
                    ConfigureApplicationPartManagerActions.ForEach(action => action.Invoke(manager));
                }).AddJsonFormatters();
        }

        private void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }
    }
}