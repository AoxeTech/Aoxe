namespace Zaaby;

public static partial class ZaabyHostExtensions
{
    public static ZaabyHost AddZaabyUnitOfWork(this ZaabyHost zaabyHost,
        Func<IServiceProvider, IDbTransaction> factory)
    {
        zaabyHost.AddScoped(factory);
        zaabyHost.Configure(app => app.UseZaabyUnitOfWork());
        return zaabyHost;
    }
}