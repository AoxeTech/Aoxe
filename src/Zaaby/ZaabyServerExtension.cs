using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Zaaby.Client;

namespace Zaaby
{
    public static class ZaabyServerExtension
    {
        private static readonly List<string> Urls = new();
        private static Action<WebHostBuilderContext, IApplicationBuilder> _configureApp;

        public static ZaabyServer UseUrls(this ZaabyServer server, params string[] urls)
        {
            Urls.AddRange(urls);
            return server;
        }

        public static ZaabyServer Configure(this ZaabyServer server,
            Action<WebHostBuilderContext, IApplicationBuilder> configureApp)
        {
            _configureApp = configureApp;
            return server;
        }

        public static void Run(this ZaabyServer server) => Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                if (server.ConfigurationServicesAction != null)
                    webBuilder.ConfigureServices(server.ConfigurationServicesAction);
                if (_configureApp != null)
                    webBuilder.Configure(_configureApp);
                if (Urls.Any())
                    webBuilder.UseUrls(Urls.ToArray());
            })
            .Build()
            .Run();

        #region Client

        public static ZaabyServer UseZaabyClient(this ZaabyServer zaabyServer,
            Dictionary<string, List<string>> baseUrls)
        {
            zaabyServer.ConfigureServices(services => { services.UseZaabyClient(baseUrls); });
            return zaabyServer;
        }

        #endregion

        #region RegisterServiceRunner

        public static ZaabyServer RegisterServiceRunner(this ZaabyServer server, Type serviceType,
            Type implementationType)
        {
            server.AddSingleton(serviceType, implementationType);
            Startup.ServiceRunnerTypes.Add(serviceType);
            return server;
        }

        public static ZaabyServer RegisterServiceRunner<TService, TImplementation>(this ZaabyServer server)
            where TImplementation : class, TService =>
            RegisterServiceRunner(server, typeof(TService), typeof(TImplementation));

        public static ZaabyServer RegisterServiceRunner(this ZaabyServer server, Type implementationType) =>
            RegisterServiceRunner(server, implementationType, implementationType);

        public static ZaabyServer RegisterServiceRunner<TService>(this ZaabyServer server, Type implementationType) =>
            RegisterServiceRunner(server, typeof(TService), implementationType);

        public static ZaabyServer RegisterServiceRunner<TImplementation>(this ZaabyServer server)
            where TImplementation : class
        {
            var implementationType = typeof(TImplementation);
            return RegisterServiceRunner(server, implementationType, implementationType);
        }

        public static ZaabyServer RegisterServiceRunner(this ZaabyServer server, Type serviceType,
            Func<IServiceProvider, object> runnerFactory)
        {
            server.AddSingleton(serviceType, runnerFactory);
            Startup.ServiceRunnerTypes.Add(serviceType);
            return server;
        }

        public static ZaabyServer RegisterServiceRunner<TService>(this ZaabyServer server,
            Func<IServiceProvider, TService> runnerFactory)
            where TService : class =>
            RegisterServiceRunner(server, typeof(TService), runnerFactory);

        #endregion
    }
}