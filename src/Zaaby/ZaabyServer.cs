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
        private readonly List<Action<IServiceCollection>> _configurationServicesActions = new();
        private readonly List<Action<IApplicationBuilder>> _configureAppActions = new();
        private readonly List<string> _urls = new();
        private Type _serviceBaseType;
        private Type _serviceAttributeType;

        public static readonly ZaabyServer Instance = new();

        private ZaabyServer()
        {
        }

        public ZaabyServer AddZaabyService<TService>() => AddZaabyService(typeof(TService));

        public ZaabyServer AddZaabyService(Type serviceDefineType)
        {
            if (typeof(Attribute).IsAssignableFrom(serviceDefineType))
                _serviceAttributeType = serviceDefineType;
            else
                _serviceBaseType = serviceDefineType;
            return Instance;
        }

        public ZaabyServer ConfigureServices(Action<IServiceCollection> configureServicesAction)
        {
            _configurationServicesActions.Add(configureServicesAction);
            return Instance;
        }

        public ZaabyServer Configure(Action<IApplicationBuilder> configureAppAction)
        {
            _configureAppActions.Add(configureAppAction);
            return Instance;
        }

        public ZaabyServer UseUrls(params string[] urls)
        {
            _urls.AddRange(urls);
            return Instance;
        }

        public void Run() => Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(services =>
                {
                    _configurationServicesActions.ForEach(action => action.Invoke(services));
                    _serviceDescriptors.ForEach(services.Add);
                    if (_serviceBaseType is not null)
                        services.AddZaabyService(_serviceBaseType);
                    if (_serviceAttributeType is not null)
                        services.AddZaabyService(_serviceAttributeType);
                });
                webBuilder.Configure(webHostBuilder =>
                {
                    _configureAppActions.ForEach(action => action.Invoke(webHostBuilder));
                    ServiceRunnerTypes.ForEach(type => webHostBuilder.ApplicationServices.GetService(type));

                    webHostBuilder.UseHttpsRedirection();
                    webHostBuilder.UseRouting();
                    webHostBuilder.UseAuthorization();
                    webHostBuilder.UseEndpoints(endpoints => { endpoints.MapControllers(); });
                });
                if (_urls.Any())
                    webBuilder.UseUrls(_urls.ToArray());
            })
            .Build()
            .Run();
    }
}