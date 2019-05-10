using System;

namespace Zaaby.Abstractions
{
    public interface IZaabyServer
    {
        IZaabyServer UseZaabyServer<TService>();
        IZaabyServer UseZaabyServer(Func<Type, bool> definition);
        IZaabyServer UseUrls(params string[] urls);
        void Run();

        #region IOC

        #region AddTransient

        IZaabyServer AddTransient(Type serviceType, Type implementationType);
        IZaabyServer AddTransient<TService, TImplementation>() where TImplementation : class, TService;
        IZaabyServer AddTransient(Type implementationType);
        IZaabyServer AddTransient<TService>(Type implementationType);
        IZaabyServer AddTransient<TImplementation>() where TImplementation : class;
        IZaabyServer AddTransient(Type serviceType, Func<IServiceProvider, object> implementationFactory);
        IZaabyServer AddTransient<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class;

        #endregion

        #region AddScope

        IZaabyServer AddScoped(Type serviceType, Type implementationType);
        IZaabyServer AddScoped<TService, TImplementation>() where TImplementation : class, TService;
        IZaabyServer AddScoped(Type serviceType);
        IZaabyServer AddScoped<TService>(Type implementationType);
        IZaabyServer AddScoped<TImplementation>() where TImplementation : class;
        IZaabyServer AddScoped(Type serviceType, Func<IServiceProvider, object> implementationFactory);
        IZaabyServer AddScoped<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class;

        #endregion

        #region AddSingleton

        IZaabyServer AddSingleton(Type serviceType, Type implementationType);
        IZaabyServer AddSingleton<TService, TImplementation>() where TImplementation : class, TService;
        IZaabyServer AddSingleton(Type serviceType);
        IZaabyServer AddSingleton<TService>(Type implementationType);
        IZaabyServer AddSingleton<TImplementation>() where TImplementation : class;
        IZaabyServer AddSingleton(Type serviceType, Func<IServiceProvider, object> implementationFactory);
        IZaabyServer AddSingleton<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class;

        #endregion

        #endregion

        #region Service runner register

        IZaabyServer RegisterServiceRunner(Type serviceType, Type implementationType);
        IZaabyServer RegisterServiceRunner<TService, TImplementation>() where TImplementation : class, TService;
        IZaabyServer RegisterServiceRunner(Type implementationType);
        IZaabyServer RegisterServiceRunner<TService>(Type implementationType);
        IZaabyServer RegisterServiceRunner<TImplementation>() where TImplementation : class;
        IZaabyServer RegisterServiceRunner(Type serviceType, Func<IServiceProvider, object> runnerFactory);
        IZaabyServer RegisterServiceRunner<TService>(Func<IServiceProvider, TService> runnerFactory)
            where TService : class;

        #endregion
    }
}