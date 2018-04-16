using System;
using System.Collections.Generic;

namespace Zaaby.Core
{
    public interface IZaabyServer
    {
        #region AddTransient

        IZaabyServer AddTransient(Type serviceType, Type implementationType);

        IZaabyServer AddTransient<TService, TImplementation>()
            where TService : class where TImplementation : class, TService;

        IZaabyServer AddTransient(Type serviceType);

        IZaabyServer AddTransient<TService>() where TService : class;

        IZaabyServer AddTransient(Type serviceType,
            Func<IServiceProvider, object> implementationFactory);

        IZaabyServer AddTransient<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class;

        IZaabyServer AddTransient<TService, TImplementation>(
            Func<IServiceProvider, TImplementation> implementationFactory) where TService : class
            where TImplementation : class, TService;

        #endregion

        #region AddScope

        IZaabyServer AddScoped(Type serviceType, Type implementationType);

        IZaabyServer AddScoped<TService, TImplementation>()
            where TService : class where TImplementation : class, TService;

        IZaabyServer AddScoped(Type serviceType);

        IZaabyServer AddScoped<TService>() where TService : class;

        IZaabyServer AddScoped(Type serviceType, Func<IServiceProvider, object> implementationFactory);

        IZaabyServer AddScoped<TService>(Func<IServiceProvider, TService> implementationFactory) where TService : class;

        IZaabyServer AddScoped<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory)
            where TService : class where TImplementation : class, TService;

        #endregion

        #region AddSingleton

        IZaabyServer AddSingleton(Type serviceType, Type implementationType);

        IZaabyServer AddSingleton<TService, TImplementation>()
            where TService : class where TImplementation : class, TService;

        IZaabyServer AddSingleton(Type serviceType);

        IZaabyServer AddSingleton<TService>() where TService : class;

        IZaabyServer AddSingleton(Type serviceType, Func<IServiceProvider, object> implementationFactory);

        IZaabyServer AddSingleton<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class;

        IZaabyServer AddSingleton<TService, TImplementation>(
            Func<IServiceProvider, TImplementation> implementationFactory) where TService : class
            where TImplementation : class, TService;

        #endregion

        IZaabyServer UseZaabyApplicationService(Dictionary<string, List<string>> baseUrls = null,
            Func<Type, bool> defineIService = null);

        IZaabyServer UseZaabyRepository(Func<Type, bool> defineIRepository = null);

        void Run();
    }
} 