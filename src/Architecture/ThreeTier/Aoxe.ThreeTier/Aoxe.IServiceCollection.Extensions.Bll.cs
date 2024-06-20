namespace Aoxe.ThreeTier;

public static partial class AoxeIServiceCollectionExtensions
{
    public static IServiceCollection AddBll(this IServiceCollection services) =>
        services
            .Register<IBll>(ServiceLifetime.Scoped)
            .Register<BllAttribute>(ServiceLifetime.Scoped);

    public static IServiceCollection AddMessageHandler(this IServiceCollection services) =>
        services
            .Register<IMessageHandler>(ServiceLifetime.Scoped)
            .Register<MessageHandlerAttribute>(ServiceLifetime.Scoped);
}
