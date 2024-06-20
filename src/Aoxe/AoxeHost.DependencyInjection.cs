namespace Aoxe;

public partial class AoxeHost
{
    private readonly List<ServiceDescriptor> _serviceDescriptors = new();
    private readonly List<ServiceDescriptor> _tryAddEnumerableDescriptors = new();

    #region AddTransient

    public AoxeHost AddTransient(Type serviceType, Type implementationType)
    {
        if (serviceType is null)
            throw new ArgumentNullException(nameof(serviceType));
        if (implementationType is null)
            throw new ArgumentNullException(nameof(implementationType));
        Add(serviceType, implementationType, ServiceLifetime.Transient);
        return Instance;
    }

    public AoxeHost AddTransient<TService, TImplementation>()
        where TImplementation : class, TService =>
        AddTransient(typeof(TService), typeof(TImplementation));

    public AoxeHost AddTransient(Type implementationType) =>
        AddTransient(implementationType, implementationType);

    public AoxeHost AddTransient<TService>(Type implementationType) =>
        AddTransient(typeof(TService), implementationType);

    public AoxeHost AddTransient<TImplementation>()
        where TImplementation : class
    {
        var implementationType = typeof(TImplementation);
        return AddTransient(implementationType, implementationType);
    }

    public AoxeHost AddTransient(
        Type serviceType,
        Func<IServiceProvider, object> implementationFactory
    )
    {
        if (serviceType is null)
            throw new ArgumentNullException(nameof(serviceType));
        if (implementationFactory is null)
            throw new ArgumentNullException(nameof(implementationFactory));
        Add(serviceType, implementationFactory, ServiceLifetime.Transient);
        return Instance;
    }

    public AoxeHost AddTransient<TService>(Func<IServiceProvider, TService> implementationFactory)
        where TService : class => AddTransient(typeof(TService), implementationFactory);

    #endregion

    #region AddScoped

    public AoxeHost AddScoped(Type serviceType, Type implementationType)
    {
        if (serviceType is null)
            throw new ArgumentNullException(nameof(serviceType));
        if (implementationType is null)
            throw new ArgumentNullException(nameof(implementationType));
        Add(serviceType, implementationType, ServiceLifetime.Scoped);
        return Instance;
    }

    public AoxeHost AddScoped<TService, TImplementation>()
        where TImplementation : class, TService =>
        AddScoped(typeof(TService), typeof(TImplementation));

    public AoxeHost AddScoped(Type serviceType) => AddScoped(serviceType, serviceType);

    public AoxeHost AddScoped<TService>(Type implementationType) =>
        AddScoped(typeof(TService), implementationType);

    public AoxeHost AddScoped<TImplementation>()
        where TImplementation : class
    {
        var implementationType = typeof(TImplementation);
        return AddScoped(implementationType, implementationType);
    }

    public AoxeHost AddScoped(
        Type serviceType,
        Func<IServiceProvider, object> implementationFactory
    )
    {
        if (serviceType is null)
            throw new ArgumentNullException(nameof(serviceType));
        if (implementationFactory is null)
            throw new ArgumentNullException(nameof(implementationFactory));
        Add(serviceType, implementationFactory, ServiceLifetime.Scoped);
        return Instance;
    }

    public AoxeHost AddScoped<TService>(Func<IServiceProvider, TService> implementationFactory)
        where TService : class => AddScoped(typeof(TService), implementationFactory);

    #endregion

    #region AddSingleton

    public AoxeHost AddSingleton(Type serviceType, Type implementationType)
    {
        if (serviceType is null)
            throw new ArgumentNullException(nameof(serviceType));
        if (implementationType is null)
            throw new ArgumentNullException(nameof(implementationType));
        Add(serviceType, implementationType, ServiceLifetime.Singleton);
        return Instance;
    }

    public AoxeHost AddSingleton<TService, TImplementation>()
        where TImplementation : class, TService =>
        AddSingleton(typeof(TService), typeof(TImplementation));

    public AoxeHost AddSingleton(Type serviceType) => AddSingleton(serviceType, serviceType);

    public AoxeHost AddSingleton<TService>(Type implementationType) =>
        AddSingleton(typeof(TService), implementationType);

    public AoxeHost AddSingleton<TImplementation>()
        where TImplementation : class
    {
        var implementationType = typeof(TImplementation);
        return AddSingleton(implementationType, implementationType);
    }

    public AoxeHost AddSingleton(
        Type serviceType,
        Func<IServiceProvider, object> implementationFactory
    )
    {
        if (serviceType is null)
            throw new ArgumentNullException(nameof(serviceType));
        if (implementationFactory is null)
            throw new ArgumentNullException(nameof(implementationFactory));
        Add(serviceType, implementationFactory, ServiceLifetime.Singleton);
        return Instance;
    }

    public AoxeHost AddSingleton<TService>(Func<IServiceProvider, TService> implementationFactory)
        where TService : class => AddSingleton(typeof(TService), implementationFactory);

    #endregion

    private void Add(Type serviceType, Type implementationType, ServiceLifetime lifetime) =>
        _serviceDescriptors.Add(new ServiceDescriptor(serviceType, implementationType, lifetime));

    private void Add(
        Type serviceType,
        Func<IServiceProvider, object> implementationFactory,
        ServiceLifetime lifetime
    ) =>
        _serviceDescriptors.Add(
            new ServiceDescriptor(serviceType, implementationFactory, lifetime)
        );

    public void TryAddEnumerable(params ServiceDescriptor[] descriptors) =>
        _tryAddEnumerableDescriptors.AddRange(descriptors);
}
