namespace Aoxe.Shared;

public static partial class AoxeIServiceCollectionExtensions
{
    public static IServiceCollection Register<TDefineType>(
        this IServiceCollection services,
        ServiceLifetime serviceLifetime
    ) => services.Register(typeof(TDefineType), serviceLifetime);

    public static IServiceCollection Register(
        this IServiceCollection services,
        Type defineType,
        ServiceLifetime serviceLifetime
    )
    {
        var typePairs = LoadHelper.GetTypePairs(defineType);
        return services.Register(typePairs, serviceLifetime);
    }

    private static IServiceCollection Register(
        this IServiceCollection services,
        IEnumerable<TypePair> typePairs,
        ServiceLifetime serviceLifetime
    )
    {
        var serviceDescriptors = typePairs.Select(
            tp => new ServiceDescriptor(tp.InterfaceType, tp.ImplementationType, serviceLifetime)
        );
        foreach (var serviceDescriptor in serviceDescriptors)
            services.Add(serviceDescriptor);
        return services;
    }
}
