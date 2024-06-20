namespace Aoxe;

public static partial class AoxeHostExtensions
{
    public static AoxeHost AddAoxeUnitOfWork(
        this AoxeHost AoxeHost,
        Func<IServiceProvider, IDbTransaction> factory
    )
    {
        AoxeHost.AddScoped(factory);
        AoxeHost.Configure(app => app.UseAoxeUnitOfWork());
        return AoxeHost;
    }
}
