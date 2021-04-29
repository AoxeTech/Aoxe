using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Zaaby.Service;

namespace Zaaby
{
    public partial class ZaabyServer
    {
        internal readonly List<Action<IServiceCollection>> ConfigurationServicesActions = new();
        internal readonly List<Action<IApplicationBuilder>> ConfigureAppActions = new();
        internal readonly List<string> Urls = new();
        internal Type ServiceBaseType;

        public static readonly ZaabyServer Instance = new();

        private ZaabyServer()
        {
        }

        public ZaabyServer AddZaabyService<TService>() => AddZaabyService(typeof(TService));

        public ZaabyServer AddZaabyService(Type serviceBaseType)
        {
            ServiceBaseType = serviceBaseType;
            return Instance;
        }

        public ZaabyServer ConfigureServices(Action<IServiceCollection> configureServicesAction)
        {
            ConfigurationServicesActions.Add(configureServicesAction);
            return Instance;
        }

        public ZaabyServer Configure(Action<IApplicationBuilder> configureAppAction)
        {
            ConfigureAppActions.Add(configureAppAction);
            return Instance;
        }

        public ZaabyServer UseUrls(params string[] urls)
        {
            Urls.AddRange(urls);
            return Instance;
        }

        public void Run() => Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(services =>
                {
                    ConfigurationServicesActions.ForEach(action => action.Invoke(services));
                    ServiceDescriptors.ForEach(services.Add);
                    if (ServiceBaseType is not null)
                        services.AddZaabyService(ServiceBaseType);
                });
                webBuilder.Configure(webHostBuilder =>
                {
                    ConfigureAppActions.ForEach(action => action.Invoke(webHostBuilder));
                    ServiceRunnerTypes.ForEach(type => webHostBuilder.ApplicationServices.GetService(type));

                    webHostBuilder.UseHttpsRedirection();
                    webHostBuilder.UseRouting();
                    webHostBuilder.UseAuthorization();
                    webHostBuilder.UseEndpoints(endpoints => { endpoints.MapControllers(); });
                });
                if (Urls.Any())
                    webBuilder.UseUrls(Urls.ToArray());
            })
            .Build()
            .Run();
    }
}