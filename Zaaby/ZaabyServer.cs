﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Zaaby.Abstractions;

namespace Zaaby
{
    public class ZaabyServer : IZaabyServer
    {
        private static readonly object LockObj = new object();
        private static ZaabyServer _zaabyServer;

        protected static readonly List<string> Urls = new List<string>();

        public static IZaabyServer GetInstance()
        {
            if (_zaabyServer == null)
            {
                lock (LockObj)
                {
                    if (_zaabyServer == null)
                        _zaabyServer = new ZaabyServer();
                }
            }

            return _zaabyServer;
        }

        public IZaabyServer UseUrls(params string[] urls)
        {
            Urls.AddRange(urls);
            return _zaabyServer;
        }

        public void Run()
        {
            var webHostBuilder = WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>();

            Urls?.ForEach(url => webHostBuilder.UseUrls(url));

            webHostBuilder.Build()
                .Run();
        }

        #region IOC

        #region AddTransient

        public IZaabyServer AddTransient(Type serviceType, Type implementationType)
        {
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));
            Add(serviceType, implementationType, ServiceLifetime.Transient);
            return _zaabyServer;
        }

        public IZaabyServer AddTransient<TService, TImplementation>() where TImplementation : class, TService
        {
            return AddTransient(typeof(TService), typeof(TImplementation));
        }

        public IZaabyServer AddTransient(Type implementationType)
        {
            return AddTransient(implementationType, implementationType);
        }

        public IZaabyServer AddTransient<TService>(Type implementationType)
        {
            return AddTransient(typeof(TService), implementationType);
        }

        public IZaabyServer AddTransient<TImplementation>() where TImplementation : class
        {
            var implementationType = typeof(TImplementation);
            return AddTransient(implementationType, implementationType);
        }

        public IZaabyServer AddTransient(Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationFactory == null) throw new ArgumentNullException(nameof(implementationFactory));
            Add(serviceType, implementationFactory, ServiceLifetime.Transient);
            return _zaabyServer;
        }

        public IZaabyServer AddTransient<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            return AddTransient(typeof(TService), implementationFactory);
        }

        #endregion

        #region AddScoped

        public IZaabyServer AddScoped(Type serviceType, Type implementationType)
        {
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));
            Add(serviceType, implementationType, ServiceLifetime.Scoped);
            return _zaabyServer;
        }

        public IZaabyServer AddScoped<TService, TImplementation>() where TImplementation : class, TService
        {
            return AddScoped(typeof(TService), typeof(TImplementation));
        }

        public IZaabyServer AddScoped(Type serviceType)
        {
            return AddScoped(serviceType, serviceType);
        }

        public IZaabyServer AddScoped<TService>(Type implementationType)
        {
            return AddScoped(typeof(TService), implementationType);
        }

        public IZaabyServer AddScoped<TImplementation>() where TImplementation : class
        {
            var implementationType = typeof(TImplementation);
            return AddScoped(implementationType, implementationType);
        }

        public IZaabyServer AddScoped(Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationFactory == null) throw new ArgumentNullException(nameof(implementationFactory));
            Add(serviceType, implementationFactory, ServiceLifetime.Scoped);
            return _zaabyServer;
        }

        public IZaabyServer AddScoped<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            return AddScoped(typeof(TService), implementationFactory);
        }

        #endregion

        #region AddSingleton

        public IZaabyServer AddSingleton(Type serviceType, Type implementationType)
        {
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationType == null) throw new ArgumentNullException(nameof(implementationType));
            Add(serviceType, implementationType, ServiceLifetime.Singleton);
            return _zaabyServer;
        }

        public IZaabyServer AddSingleton<TService, TImplementation>() where TImplementation : class, TService
        {
            return AddSingleton(typeof(TService), typeof(TImplementation));
        }

        public IZaabyServer AddSingleton(Type serviceType)
        {
            return AddSingleton(serviceType, serviceType);
        }

        public IZaabyServer AddSingleton<TService>(Type implementationType)
        {
            return AddSingleton(typeof(TService), implementationType);
        }

        public IZaabyServer AddSingleton<TImplementation>() where TImplementation : class
        {
            var implementationType = typeof(TImplementation);
            return AddSingleton(implementationType, implementationType);
        }

        public IZaabyServer AddSingleton(Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            if (serviceType == null) throw new ArgumentNullException(nameof(serviceType));
            if (implementationFactory == null) throw new ArgumentNullException(nameof(implementationFactory));
            Add(serviceType, implementationFactory, ServiceLifetime.Singleton);
            return _zaabyServer;
        }

        public IZaabyServer AddSingleton<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            return AddSingleton(typeof(TService), implementationFactory);
        }

        #endregion

        #endregion

        #region RegisterServiceRunner

        public IZaabyServer RegisterServiceRunner(Type serviceType, Type implementationType)
        {
            Add(serviceType, implementationType, ServiceLifetime.Singleton);
            Startup.ServiceRunnerTypes.Add(serviceType);
            return _zaabyServer;
        }

        public IZaabyServer RegisterServiceRunner<TService, TImplementation>() where TImplementation : class, TService=>
            RegisterServiceRunner(typeof(TService), typeof(TImplementation));

        public IZaabyServer RegisterServiceRunner(Type implementationType)=>
            RegisterServiceRunner(implementationType, implementationType);

        public IZaabyServer RegisterServiceRunner<TService>(Type implementationType)=>
            RegisterServiceRunner(typeof(TService), implementationType);

        public IZaabyServer RegisterServiceRunner<TImplementation>() where TImplementation : class
        {
            var implementationType = typeof(TImplementation);
            return RegisterServiceRunner(implementationType, implementationType);
        }

        public IZaabyServer RegisterServiceRunner(Type serviceType, Func<IServiceProvider, object> runnerFactory)
        {
            Add(serviceType, runnerFactory, ServiceLifetime.Singleton);
            Startup.ServiceRunnerTypes.Add(serviceType);
            return _zaabyServer;
        }

        public IZaabyServer RegisterServiceRunner<TService>(Func<IServiceProvider, TService> runnerFactory)
            where TService : class =>
            RegisterServiceRunner(typeof(TService), runnerFactory);

        #endregion

        public IZaabyServer UseZaabyServer<TService>() =>
            UseZaabyServer(type =>
                type.IsInterface && typeof(TService).IsAssignableFrom(type) && type != typeof(TService));

        public IZaabyServer UseZaabyServer(Func<Type, bool> definition)
        {
            var allTypes = LoadHelper.GetAllTypes();
            var interfaceTypes = allTypes.Where(definition).ToList();

            var implementTypes = allTypes
                .Where(type => type.IsClass && interfaceTypes.Any(i => i.IsAssignableFrom(type))).ToList();

            implementTypes.ForEach(implementType =>
                Startup.ServiceDic.Add(interfaceTypes.First(i => i.IsAssignableFrom(implementType)), implementType));

            return _zaabyServer;
        }

        private static void Add(Type serviceType, Type implementationType, ServiceLifetime lifetime) =>
            Startup.ServiceDescriptors.Add(new ServiceDescriptor(serviceType, implementationType, lifetime));

        private static void Add(Type serviceType, Func<IServiceProvider, object> implementationFactory,
            ServiceLifetime lifetime) =>
            Startup.ServiceDescriptors.Add(new ServiceDescriptor(serviceType, implementationFactory, lifetime));
    }
}