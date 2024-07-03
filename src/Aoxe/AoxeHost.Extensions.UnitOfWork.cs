namespace Aoxe;

public static partial class AoxeHostExtensions
{
    public static AoxeHost AddAoxeUnitOfWork(
        this AoxeHost aoxeHost,
        Func<IServiceProvider, IDbTransaction> factory
    )
    {
        aoxeHost.AddScoped(factory);
        aoxeHost.Configure(app => app.UseAoxeUnitOfWork());
        return aoxeHost;
    }
}
