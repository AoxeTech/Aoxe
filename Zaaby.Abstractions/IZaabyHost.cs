using System;
using System.Collections.Generic;

namespace Zaaby.Abstractions
{
    public interface IZaabyHost
    {
        List<Type> AllTypes { get; set; }

        #region AddTransient

        IZaabyHost AddTransient(Type serviceType, Type implementationType);

        IZaabyHost AddTransient<TService, TImplementation>()
            where TService : class where TImplementation : class, TService;

        IZaabyHost AddTransient(Type serviceType);

        IZaabyHost AddTransient<TService>() where TService : class;

        IZaabyHost AddTransient(Type serviceType,
            Func<IServiceProvider, object> implementationFactory);

        IZaabyHost AddTransient<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class;

        IZaabyHost AddTransient<TService, TImplementation>(
            Func<IServiceProvider, TImplementation> implementationFactory) where TService : class
            where TImplementation : class, TService;

        #endregion

        #region AddScope

        IZaabyHost AddScoped(Type serviceType, Type implementationType);

        IZaabyHost AddScoped<TService, TImplementation>()
            where TService : class where TImplementation : class, TService;

        IZaabyHost AddScoped(Type serviceType);

        IZaabyHost AddScoped<TService>() where TService : class;

        IZaabyHost AddScoped(Type serviceType, Func<IServiceProvider, object> implementationFactory);

        IZaabyHost AddScoped<TService>(Func<IServiceProvider, TService> implementationFactory) where TService : class;

        IZaabyHost AddScoped<TService, TImplementation>(Func<IServiceProvider, TImplementation> implementationFactory)
            where TService : class where TImplementation : class, TService;

        #endregion

        #region AddSingleton

        IZaabyHost AddSingleton(Type serviceType, Type implementationType);

        IZaabyHost AddSingleton<TService, TImplementation>()
            where TService : class where TImplementation : class, TService;

        IZaabyHost AddSingleton(Type serviceType);

        IZaabyHost AddSingleton<TService>() where TService : class;

        IZaabyHost AddSingleton(Type serviceType, Func<IServiceProvider, object> implementationFactory);

        IZaabyHost AddSingleton<TService>(Func<IServiceProvider, TService> implementationFactory)
            where TService : class;

        IZaabyHost AddSingleton<TService, TImplementation>(
            Func<IServiceProvider, TImplementation> implementationFactory) where TService : class
            where TImplementation : class, TService;

        #endregion

        #region Service runner register

        IZaabyHost RegisterServiceRunners(List<Type> runnerTypes);

        IZaabyHost RegisterServiceRunner(Type runnerType);

        IZaabyHost RegisterServiceRunners(Dictionary<Type, Type> runnerTypes);

        IZaabyHost RegisterServiceRunner(Type serviceType, Type implementationType);

        #endregion
    }
}