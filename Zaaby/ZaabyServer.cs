using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Zaaby.Core;

namespace Zaaby
{
    public class ZaabyServer : IZaabyServer
    {
        private static readonly object LockObj = new object();
        private static ZaabyServer _zaabyServer;
        private static List<Type> _allTypes;

        private static Action _useDynamicProxyAction;
        private static Action _useAppService;
        private static Action _useRepository;

        private static readonly List<Action<MvcOptions>> AddMvcCoreActions = new List<Action<MvcOptions>>();

        private static readonly List<Action<ApplicationPartManager>> ConfigureApplicationPartManagerActions =
            new List<Action<ApplicationPartManager>>();
        
        private static readonly List<string> Urls = new List<string>();

        private static readonly Dictionary<Type, Type> ScopeDic = new Dictionary<Type, Type>();
        private static readonly Dictionary<Type, Type> TransientDic = new Dictionary<Type, Type>();
        private static readonly Dictionary<Type, Type> SingletonDic = new Dictionary<Type, Type>();
        private static readonly List<ServiceDescriptor> ServiceDescriptors = new List<ServiceDescriptor>();

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
            _useRepository?.Invoke();
            _useDynamicProxyAction?.Invoke();

            var webHostBuilder = WebHost.CreateDefaultBuilder()
                .ConfigureServices(ConfigureServices)
                .Configure(Configure);

            if (Urls.Count > 0)
                Urls.ForEach(url => webHostBuilder.UseUrls(url));

            webHostBuilder.Build()
                .Run();
        }

        public IZaabyServer UseZaabyApplicationService(Dictionary<string, List<string>> baseUrls = null,
            Func<Type, bool> defineIService = null)
        {
            var allInterfaces = _allTypes.Where(type => type.IsInterface);
            var serviceInterfaces = defineIService != null
                ? allInterfaces.Where(defineIService)
                : allInterfaces.Where(type =>
                        typeof(IZaabyAppService).IsAssignableFrom(type) && type != typeof(IZaabyAppService))
                    .ToList();
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
                AddMvcCoreActions.Add(mvcOptions =>
                {
                    foreach (var serviceInterface in serviceInterfaces)
                        mvcOptions.Conventions.Add(new ZaabyActionModelConvention(serviceInterface));
                });
                ConfigureApplicationPartManagerActions.Add(manager =>
                {
                    manager.FeatureProviders
                        .Add(new ZaabyAppServiceControllerFeatureProvider(implementServices));
                });
            };
            if (baseUrls != null)
                _useDynamicProxyAction = () =>
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
                };
            return _zaabyServer;
        }

        public IZaabyServer UseZaabyRepository(Func<Type, bool> defineIRepository = null)
        {
            var allInterfaces = _allTypes.Where(type => type.IsInterface);
            var repositoryInterfaces = defineIRepository != null
                ? allInterfaces.Where(defineIRepository)
                : allInterfaces.Where(type =>
                        typeof(IZaabyRepository).IsAssignableFrom(type) && type != typeof(IZaabyRepository))
                    .ToList();
            var implementRepositories = _allTypes
                .Where(type => type.IsClass && repositoryInterfaces.Any(i => i.IsAssignableFrom(type)))
                .ToList();
            _useRepository = () =>
            {
                implementRepositories.ForEach(repository =>
                {
                    AddScoped(repository.GetInterfaces().First(i => repositoryInterfaces.Contains(i)),
                        repository);
                });
            };
            return _zaabyServer;
        }

        private void ConfigureServices(IServiceCollection services)
        {
            foreach (var keyValuePair in TransientDic)
                services.AddTransient(keyValuePair.Key, keyValuePair.Value);

            foreach (var keyValuePair in ScopeDic)
                services.AddScoped(keyValuePair.Key, keyValuePair.Value);

            foreach (var keyValuePair in SingletonDic)
                services.AddSingleton(keyValuePair.Key, keyValuePair.Value);

            services.Add(ServiceDescriptors);

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
                    SingletonDic.TryAdd(serviceType, implementationType);
                    break;
                case ServiceLifetime.Scoped:
                    ScopeDic.TryAdd(serviceType, implementationType);
                    break;
                case ServiceLifetime.Transient:
                    TransientDic.TryAdd(serviceType, implementationType);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lifetime), lifetime, null);
            }
        }

        private void Add(Type serviceType, Func<IServiceProvider, object> implementationFactory,
            ServiceLifetime lifetime)
        {
            ServiceDescriptors.Add(new ServiceDescriptor(serviceType, implementationFactory, lifetime));
        }

        #endregion
    }
}