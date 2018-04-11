using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Zaaby.Core;

namespace Zaaby
{
    public class ZaabyServer : IZaabyServer
    {
        private static readonly object LockObj = new object();
        private static ZaabyServer _zaabyServer;

        private static List<Type> _serviceInterfaces;
        private static List<Type> _implementServices;

        private static List<Type> _repositoryInterfaces;
        private static List<Type> _implementrepositories;
        
        private Func<Type, bool> _defineIService;
        private Func<Type, bool> _defineIRepository;
        private Action _useDynamicProxyAction;

        private static readonly Dictionary<Type, Type> ScopeDic = new Dictionary<Type, Type>();
        private static readonly Dictionary<Type, Type> TransientDic = new Dictionary<Type, Type>();
        private static readonly Dictionary<Type, Type> SingletonDic = new Dictionary<Type, Type>();

        public static ZaabyServer GetInstance()
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

        public IZaabyServer DefineIService(Func<Type, bool> defineIService)
        {
            _defineIService = defineIService;
            return _zaabyServer;
        }

        public IZaabyServer DefineIRepository(Func<Type, bool> defineIRepository)
        {
            _defineIRepository = defineIRepository;
            return _zaabyServer;
        }

        public IZaabyServer UseDynamicProxy(Dictionary<string, List<string>> baseUrls)
        {
            _useDynamicProxyAction = () =>
            {
                var interfaces = _serviceInterfaces.Where(i =>
                    _implementServices.All(s => !i.IsAssignableFrom(s))).ToList();

                var dynamicProxy = new ZaabyDynamicProxy(interfaces
                    .Where(@interface => !baseUrls.ContainsKey(@interface.FullName))
                    .ToDictionary(k => k, v => baseUrls[v.FullName]));
                var type = dynamicProxy.GetType();
                var methods = type.GetMethods();
                var methodInfo = type.GetMethod("GetService").MakeGenericMethod(type);
                foreach (var interfaceType in interfaces)
                {
                    var g = methodInfo.MakeGenericMethod(interfaceType);
                    g.Invoke(dynamicProxy, null);
                }
            };

            return _zaabyServer;
        }

        private void InitApplicationServiceType(IReadOnlyCollection<Type> allTypes)
        {
            var interfaceQuery = allTypes.Where(type => type.IsInterface);
            
            var serviceQuery = _defineIService != null
                ? interfaceQuery.Where(_defineIService)
                : interfaceQuery.Where(type => typeof(IZaabyAppService).IsAssignableFrom(type));
            _serviceInterfaces = serviceQuery.ToList();
            _implementServices = allTypes
                .Where(type => type.IsClass && _serviceInterfaces.Any(i => i.IsAssignableFrom(type)))
                .ToList();
        }

        private void InitRepositoryType(List<Type> allTypes)
        {
            var interfaceQuery = allTypes.Where(type => type.IsInterface);

            var repositoryQuery = _defineIRepository != null
                ? interfaceQuery.Where(_defineIRepository)
                : interfaceQuery.Where(type => typeof(IZaabyRepository<,>).IsAssignableFrom(type));
            _repositoryInterfaces = repositoryQuery.ToList();
            _implementrepositories = allTypes
                .Where(type => type.IsClass && _repositoryInterfaces.Any(i => i.IsAssignableFrom(type)))
                .ToList();
        }

        public void Run()
        {
            var allTypes = GetTypes();
            
            InitApplicationServiceType(allTypes);

            InitRepositoryType(allTypes);
            
            _useDynamicProxyAction?.Invoke();
            WebHost.CreateDefaultBuilder()
                .ConfigureServices(ConfigureServices)
                .Configure(Configure)
                .Build()
                .Run();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            _implementServices.ForEach(service =>
            {
                var serviceInterface =
                    service.GetInterfaces().FirstOrDefault(i => _serviceInterfaces.Contains(i));
                if (serviceInterface != null)
                    services.AddScoped(serviceInterface, service);
            });
            
            _implementrepositories.ForEach(repository =>
            {
                var repositoryInterface =
                    repository.GetInterfaces().FirstOrDefault(i => _repositoryInterfaces.Contains(i));
                if (repositoryInterface != null)
                    services.AddScoped(repositoryInterface, repository);
            });

            foreach (var keyValuePair in TransientDic)
                services.AddTransient(keyValuePair.Key, keyValuePair.Value);

            foreach (var keyValuePair in ScopeDic)
                services.AddScoped(keyValuePair.Key, keyValuePair.Value);

            foreach (var keyValuePair in SingletonDic)
                services.AddSingleton(keyValuePair.Key, keyValuePair.Value);

            services.AddMvcCore(mvcOptions =>
                {
                    _serviceInterfaces.ForEach(type =>
                    {
                        mvcOptions.Conventions.Add(new ZaabyActionModelConvention(type));
                    });
                })
                .ConfigureApplicationPartManager(manager =>
                {
                    manager.FeatureProviders.Add(
                        new ZaabyAppServiceControllerFeatureProvider(_implementServices));
                }).AddJsonFormatters();
        }

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

        private void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
        }

        private static List<Type> GetTypes()
        {
            var dir = Directory.GetCurrentDirectory();
            var files = new List<string>();

            files.AddRange(Directory.GetFiles(dir + @"\", "*.dll", SearchOption.AllDirectories));
            files.AddRange(Directory.GetFiles(dir + @"\", "*.exe", SearchOption.AllDirectories));

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
            var implementationType = implementationFactory.GetType().GenericTypeArguments[1];
            Add(serviceType, implementationType, lifetime);
        }

        private ZaabyServer()
        {
        }
    }
}