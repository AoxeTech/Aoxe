﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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
        public List<Type> AllTypes { get; set; }

        private static readonly List<string> Urls = new List<string>();

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

            if (Urls.Count > 0)
                Urls.ForEach(url => webHostBuilder.UseUrls(url));

            webHostBuilder.Build()
                .Run();
        }

        public IZaabyServer UseZaabyServer<TService>()
        {
            var allInterfaces = AllTypes.Where(type => type.IsInterface);
            var interfaceTypes = allInterfaces.Where(type =>
                typeof(TService).IsAssignableFrom(type)).ToList();
            var implementTypes = AllTypes
                .Where(type => type.IsClass && interfaceTypes.Any(i => i.IsAssignableFrom(type))).ToList();

            implementTypes.ForEach(implementType =>
                Startup.ServiceDic.TryAdd(
                    interfaceTypes.FirstOrDefault(interfaceType =>
                        interfaceType.IsAssignableFrom(implementType) && interfaceType != typeof(TService)) ??
                    interfaceTypes.First(interfaceType => interfaceType.IsAssignableFrom(implementType)),
                    implementType));

            return _zaabyServer;
        }

        public IZaabyServer RegisterServiceRunners(List<Type> runnerTypes)
        {
            runnerTypes.ForEach(type => RegisterServiceRunner(type));
            return _zaabyServer;
        }

        public IZaabyServer RegisterServiceRunner(Type runnerType)
        {
            AddSingleton(runnerType);
            Startup.ServiceRunnerTypes.Add(runnerType);
            return _zaabyServer;
        }

        private ZaabyServer()
        {
            AllTypes = GetAllTypes();
        }

        private static List<Type> GetAllTypes()
        {
            var dir = Directory.GetCurrentDirectory();
            var files = new List<string>();

            files.AddRange(Directory.GetFiles(dir + @"/", "*.dll", SearchOption.AllDirectories));
            files.AddRange(Directory.GetFiles(dir + @"/", "*.exe", SearchOption.AllDirectories));

            var typeDic = new Dictionary<string, Type>();

            foreach (var file in files)
            {
                try
                {
                    foreach (var type in Assembly.LoadFrom(file).GetTypes())
                        if (!typeDic.ContainsKey(type.FullName))
                            typeDic.Add(type.FullName, type);
                }
                catch (BadImageFormatException)
                {
                    // ignored
                }
            }

            return typeDic.Select(kv => kv.Value).ToList();
        }

        #region IOC

        #region AddTransient

        public IZaabyServer AddTransient(Type serviceType, Type implementationType)
        {
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));
            if (implementationType == null)
                throw new ArgumentNullException(nameof(implementationType));
            Add(serviceType, implementationType, ServiceLifetime.Transient);
            return _zaabyServer;
        }

        public IZaabyServer AddTransient<TService, TImplementation>()
            where TService : class where TImplementation : class, TService
        {
            Add(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient);
            return _zaabyServer;
        }

        public IZaabyServer AddTransient(Type serviceType)
        {
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));
            Add(serviceType, serviceType, ServiceLifetime.Transient);
            return _zaabyServer;
        }

        public IZaabyServer AddTransient<TService>() where TService : class
        {
            Add(typeof(TService), typeof(TService), ServiceLifetime.Transient);
            return _zaabyServer;
        }

        public IZaabyServer AddTransient(Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));
            if (implementationFactory == null)
                throw new ArgumentNullException(nameof(implementationFactory));
            Add(serviceType, implementationFactory, ServiceLifetime.Transient);
            return _zaabyServer;
        }

        public IZaabyServer AddTransient<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            if (implementationFactory == null)
                throw new ArgumentNullException(nameof(implementationFactory));
            Add(typeof(TService), implementationFactory, ServiceLifetime.Transient);
            return _zaabyServer;
        }

        public IZaabyServer AddTransient<TService, TImplementation>(
            Func<IServiceProvider, TImplementation> implementationFactory) where TService : class
            where TImplementation : class, TService
        {
            if (implementationFactory == null)
                throw new ArgumentNullException(nameof(implementationFactory));
            Add(typeof(TService), implementationFactory, ServiceLifetime.Transient);
            return _zaabyServer;
        }

        #endregion

        #region AddScope

        public IZaabyServer AddScoped(Type serviceType, Type implementationType)
        {
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));
            if (implementationType == null)
                throw new ArgumentNullException(nameof(implementationType));
            Add(serviceType, implementationType, ServiceLifetime.Scoped);
            return _zaabyServer;
        }

        public IZaabyServer AddScoped<TService, TImplementation>()
            where TService : class where TImplementation : class, TService
        {
            Add(typeof(TService), typeof(TImplementation), ServiceLifetime.Scoped);
            return _zaabyServer;
        }

        public IZaabyServer AddScoped(Type serviceType)
        {
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));
            Add(serviceType, serviceType, ServiceLifetime.Scoped);
            return _zaabyServer;
        }

        public IZaabyServer AddScoped<TService>() where TService : class
        {
            Add(typeof(TService), typeof(TService), ServiceLifetime.Scoped);
            return _zaabyServer;
        }

        public IZaabyServer AddScoped(Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));
            if (implementationFactory == null)
                throw new ArgumentNullException(nameof(implementationFactory));
            Add(serviceType, implementationFactory, ServiceLifetime.Scoped);
            return _zaabyServer;
        }

        public IZaabyServer AddScoped<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            if (implementationFactory == null)
                throw new ArgumentNullException(nameof(implementationFactory));
            Add(typeof(TService), implementationFactory, ServiceLifetime.Scoped);
            return _zaabyServer;
        }

        public IZaabyServer AddScoped<TService, TImplementation>(
            Func<IServiceProvider, TImplementation> implementationFactory) where TService : class
            where TImplementation : class, TService
        {
            if (implementationFactory == null)
                throw new ArgumentNullException(nameof(implementationFactory));
            Add(typeof(TService), implementationFactory, ServiceLifetime.Scoped);
            return _zaabyServer;
        }

        #endregion

        #region AddSingleton

        public IZaabyServer AddSingleton(Type serviceType, Type implementationType)
        {
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));
            if (implementationType == null)
                throw new ArgumentNullException(nameof(implementationType));
            Add(serviceType, implementationType, ServiceLifetime.Singleton);
            return _zaabyServer;
        }

        public IZaabyServer AddSingleton<TService, TImplementation>()
            where TService : class where TImplementation : class, TService
        {
            Add(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton);
            return _zaabyServer;
        }

        public IZaabyServer AddSingleton(Type serviceType)
        {
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));
            Add(serviceType, serviceType, ServiceLifetime.Singleton);
            return _zaabyServer;
        }

        public IZaabyServer AddSingleton<TService>() where TService : class
        {
            Add(typeof(TService), typeof(TService), ServiceLifetime.Singleton);
            return _zaabyServer;
        }

        public IZaabyServer AddSingleton(Type serviceType, Func<IServiceProvider, object> implementationFactory)
        {
            if (serviceType == null)
                throw new ArgumentNullException(nameof(serviceType));
            if (implementationFactory == null)
                throw new ArgumentNullException(nameof(implementationFactory));
            Add(serviceType, implementationFactory, ServiceLifetime.Singleton);
            return _zaabyServer;
        }

        public IZaabyServer AddSingleton<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class
        {
            if (implementationFactory == null)
                throw new ArgumentNullException(nameof(implementationFactory));
            Add(typeof(TService), implementationFactory, ServiceLifetime.Singleton);
            return _zaabyServer;
        }

        public IZaabyServer AddSingleton<TService, TImplementation>(
            Func<IServiceProvider, TImplementation> implementationFactory) where TService : class
            where TImplementation : class, TService
        {
            if (implementationFactory == null)
                throw new ArgumentNullException(nameof(implementationFactory));
            Add(typeof(TService), implementationFactory, ServiceLifetime.Singleton);
            return _zaabyServer;
        }

        #endregion

        private void Add(Type serviceType, Type implementationType, ServiceLifetime lifetime)
        {
            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    Startup.SingletonDic.TryAdd(serviceType, implementationType);
                    break;
                case ServiceLifetime.Scoped:
                    Startup.ScopeDic.TryAdd(serviceType, implementationType);
                    break;
                case ServiceLifetime.Transient:
                    Startup.TransientDic.TryAdd(serviceType, implementationType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null);
            }
        }

        private static void Add(Type serviceType, Func<IServiceProvider, object> implementationFactory,
            ServiceLifetime lifetime)
        {
            Startup.ServiceDescriptors.Add(new ServiceDescriptor(serviceType, implementationFactory, lifetime));
        }

        #endregion
    }
}