using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Zaaby.Core;
using Zaaby.Core.Application;
using Zaaby.Core.Domain;
using Zaaby.Core.Infrastructure.Repository;

namespace Zaaby
{
    public class ZaabyServer : IZaabyServer
    {
        private static readonly object LockObj = new object();
        private static ZaabyServer _zaabyServer;
        private static List<Type> _allTypes;

        private static Action _useAppService;

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
            _useAppService?.Invoke();

            var webHostBuilder = WebHost.CreateDefaultBuilder()
                .UseStartup<Startup>();

            if (Urls.Count > 0)
                Urls.ForEach(url => webHostBuilder.UseUrls(url));

            webHostBuilder.Build()
                .Run();
        }

        public IZaabyServer UseZaabyApplicationService(Dictionary<string, List<string>> baseUrls = null,
            Func<Type, bool> applicationServiceInterfaceDefine = null)
        {
            var allInterfaces = _allTypes.Where(type => type.IsInterface);
            var serviceInterfaces = applicationServiceInterfaceDefine != null
                ? allInterfaces.Where(applicationServiceInterfaceDefine)
                : allInterfaces.Where(type =>
                    typeof(IApplicationService).IsAssignableFrom(type) &&
                    type != typeof(IApplicationService));
            var implementServices = _allTypes
                .Where(type => type.IsClass && serviceInterfaces.Any(i => i.IsAssignableFrom(type)))
                .ToList();
            _useAppService = () =>
            {
                implementServices.ForEach(service =>
                {
                    var serviceInterface =
                        service.GetInterfaces().FirstOrDefault(i => serviceInterfaces.Contains(i));
                    if (serviceInterface != null)
                        AddScoped(serviceInterface, service);
                });
                Startup.AddMvcCoreActions.Add(mvcOptions =>
                {
                    foreach (var serviceInterface in serviceInterfaces)
                        mvcOptions.Conventions.Add(new ZaabyActionModelConvention(serviceInterface));
                });
                Startup.ConfigureApplicationPartManagerActions.Add(manager =>
                {
                    manager.FeatureProviders
                        .Add(new ZaabyAppServiceControllerFeatureProvider(implementServices));
                });
            };
            if (baseUrls != null)
            {
                var interfaces = serviceInterfaces.Where(i =>
                    implementServices.All(s => !i.IsAssignableFrom(s))).ToList();

                var dynamicProxy = new ZaabyDynamicProxy(interfaces
                    .Where(@interface => baseUrls.ContainsKey(@interface.Namespace))
                    .Select(@interface => @interface.Namespace)
                    .Distinct()
                    .ToDictionary(k => k, v => baseUrls[v]));
                var type = dynamicProxy.GetType();
                var methodInfo = type.GetMethod("GetService");
                foreach (var interfaceType in interfaces)
                {
                    var proxy = methodInfo.MakeGenericMethod(interfaceType).Invoke(dynamicProxy, null);
                    AddTransient(interfaceType, p => proxy);
                }
            }

            return _zaabyServer;
        }

        public IZaabyServer UseZaabyDomainService(Func<Type, bool> domainServiceInterfaceDefine = null)
        {
            var domainServiceQuery = _allTypes.Where(type => type.IsClass);
            domainServiceQuery = domainServiceInterfaceDefine == null
                ? domainServiceQuery.Where(type => typeof(IDomainService).IsAssignableFrom(type))
                : domainServiceQuery.Where(domainServiceInterfaceDefine);

            var domainServices = domainServiceQuery.ToList();

            domainServices.ForEach(domainService => AddScoped(domainService, domainService));
            return _zaabyServer;
        }

        public IZaabyServer UseZaabyDomainEventHandler()
        {
            var domainEventHandlerQuery = _allTypes
                .Where(type => type.IsClass && typeof(IDomainEventHandler<,>).IsAssignableFrom(type))
                .ToList();

            var domainEventHandlers = domainEventHandlerQuery.ToList();
            domainEventHandlers.ForEach(domainEventHandler => AddSingleton(domainEventHandler, domainEventHandler));

            return _zaabyServer;
        }

        public IZaabyServer UseZaabyRepository(Func<Type, bool> repositoryInterfaceDefine = null)
        {
            var allInterfaces = _allTypes.Where(type => type.IsInterface);
            var repositoryInterfaces = repositoryInterfaceDefine != null
                ? allInterfaces.Where(repositoryInterfaceDefine)
                : allInterfaces.Where(type =>
                    typeof(IRepository<,>).IsAssignableFrom(type) &&
                    type != typeof(IRepository<,>));
            var implementRepositories = _allTypes
                .Where(type => type.IsClass && repositoryInterfaces.Any(i => i.IsAssignableFrom(type)))
                .ToList();

            implementRepositories.ForEach(repository =>
            {
                AddScoped(repository.GetInterfaces().First(i => repositoryInterfaces.Contains(i)),
                    repository);
            });

            return _zaabyServer;
        }

        private ZaabyServer()
        {
            _allTypes = GetAllTypes();
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

        private void Add(Type serviceType, Func<IServiceProvider, object> implementationFactory,
            ServiceLifetime lifetime)
        {
            Startup.ServiceDescriptors.Add(new ServiceDescriptor(serviceType, implementationFactory, lifetime));
        }

        #endregion
    }
}