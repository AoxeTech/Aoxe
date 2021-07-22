using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Zaaby.Service;

namespace Zaaby
{
    public partial class ZaabyHost
    {
        private readonly List<Action<IServiceCollection>> _configurationServicesActions = new();
        private readonly List<Action<IApplicationBuilder>> _configureAppActions = new();
        private readonly List<string> _urls = new();
        private readonly List<Type> _serviceBaseTypes = new();
        private readonly List<Type> _serviceAttributeTypes = new();

        public static readonly ZaabyHost Instance = new();

        private ZaabyHost()
        {
        }

        public ZaabyHost AddZaabyService<TService>() => AddZaabyService(typeof(TService));

        public ZaabyHost AddZaabyService(Type serviceDefineType)
        {
            if (typeof(Attribute).IsAssignableFrom(serviceDefineType)) _serviceAttributeTypes.Add(serviceDefineType);
            else _serviceBaseTypes.Add(serviceDefineType);
            return Instance;
        }

        public ZaabyHost ConfigureServices(Action<IServiceCollection> configureServicesAction)
        {
            _configurationServicesActions.Add(configureServicesAction);
            return Instance;
        }

        public ZaabyHost Configure(Action<IApplicationBuilder> configureAppAction)
        {
            _configureAppActions.Add(configureAppAction);
            return Instance;
        }

        public ZaabyHost UseUrls(params string[] urls)
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
                    _tryAddEnumerableDescriptors.ForEach(services.TryAddEnumerable);
                    _serviceBaseTypes.ForEach(serviceBaseType =>
                        services.AddZaabyService(serviceBaseType));
                    _serviceAttributeTypes.ForEach(serviceAttributeType =>
                        services.AddZaabyService(serviceAttributeType));
                });
                webBuilder.Configure(webHostBuilder =>
                {
                    webHostBuilder.UseHttpsRedirection();
                    _configureAppActions.ForEach(action => action.Invoke(webHostBuilder));
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