namespace Zaaby.Server;

public static partial class ZaabyIServiceCollectionExtensions
{
    public static IServiceCollection AddZaabyService<TService>(this IServiceCollection services) =>
        services.AddZaabyServices(typeof(TService));

    public static IServiceCollection AddZaabyService<T0, T1>(this IServiceCollection services) =>
        services.AddZaabyServices(typeof(T0), typeof(T1));

    public static IServiceCollection AddZaabyService<T0, T1, T2>(this IServiceCollection services) =>
        services.AddZaabyServices(typeof(T0), typeof(T1), typeof(T2));

    public static IServiceCollection AddZaabyService<T0, T1, T2, T3>(this IServiceCollection services) =>
        services.AddZaabyServices(typeof(T0), typeof(T1), typeof(T2), typeof(T3));

    public static IServiceCollection AddZaabyService<T0, T1, T2, T3, T4>(this IServiceCollection services) =>
        services.AddZaabyServices(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4));

    public static IServiceCollection AddZaabyService<T0, T1, T2, T3, T4, T5>(this IServiceCollection services) =>
        services.AddZaabyServices(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));

    public static IServiceCollection
        AddZaabyService<T0, T1, T2, T3, T4, T5, T6>(this IServiceCollection services) =>
        services.AddZaabyServices(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5),
            typeof(T6));

    public static IServiceCollection AddZaabyService<T0, T1, T2, T3, T4, T5, T6, T7>(
        this IServiceCollection services) =>
        services.AddZaabyServices(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5),
            typeof(T6), typeof(T7));

    public static IServiceCollection AddZaabyService<T0, T1, T2, T3, T4, T5, T6, T7, T8>(
        this IServiceCollection services) =>
        services.AddZaabyServices(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5),
            typeof(T6), typeof(T7), typeof(T8));

    public static IServiceCollection AddZaabyService<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>(
        this IServiceCollection services) =>
        services.AddZaabyServices(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5),
            typeof(T6), typeof(T7), typeof(T8), typeof(T9));

    public static IServiceCollection AddZaabyServices(this IServiceCollection services,
        params Type[] serviceDefineTypes)
    {
        var typePairs = LoadHelper.GetTypePairs(serviceDefineTypes);

        services.AddControllers(options =>
            {
                foreach (var type in typePairs.Where(t => t.ImplementationType is not null)
                             .Select(t => t.InterfaceType ?? t.ImplementationType))
                    options.Conventions.Add(new ZaabyActionModelConvention(type));
            })
            .ConfigureApplicationPartManager(manager =>
            {
                manager.FeatureProviders.Add(
                    new ZaabyAppServiceControllerFeatureProvider(typePairs
                        .Where(t => t.ImplementationType is not null)
                        .Select(t => t.ImplementationType)
                        .ToList()));
            });
        return services;
    }
}