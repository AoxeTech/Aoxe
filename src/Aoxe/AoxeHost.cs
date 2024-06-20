namespace Aoxe;

public partial class AoxeHost
{
    private readonly List<Action<IServiceCollection>> _configurationServicesActions = new();
    private readonly List<Action<IApplicationBuilder>> _configureAppActions = new();
    private readonly List<string> _urls = new();
    private readonly List<Type> _serviceBaseTypes = new();
    private readonly List<Type> _serviceAttributeTypes = new();

    public static readonly AoxeHost Instance = new();

    private AoxeHost() { }

    public AoxeHost AddAoxeService<TService>() => AddAoxeService(typeof(TService));

    public AoxeHost AddAoxeService(Type serviceDefineType)
    {
        if (typeof(Attribute).IsAssignableFrom(serviceDefineType))
            _serviceAttributeTypes.Add(serviceDefineType);
        else
            _serviceBaseTypes.Add(serviceDefineType);
        return Instance;
    }

    public AoxeHost ConfigureServices(Action<IServiceCollection> configureServicesAction)
    {
        _configurationServicesActions.Add(configureServicesAction);
        return Instance;
    }

    public AoxeHost Configure(Action<IApplicationBuilder> configureAppAction)
    {
        _configureAppActions.Add(configureAppAction);
        return Instance;
    }

    public AoxeHost UseUrls(params string[] urls)
    {
        _urls.AddRange(urls);
        return Instance;
    }

    public void Run() =>
        Host.CreateDefaultBuilder()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureServices(services =>
                {
                    _configurationServicesActions.ForEach(action => action.Invoke(services));
                    services.Add(_serviceDescriptors);
                    services.TryAddEnumerable(_tryAddEnumerableDescriptors);
                    services.AddAoxeServices(_serviceBaseTypes.ToArray());
                    services.AddAoxeServices(_serviceAttributeTypes.ToArray());
                });
                webBuilder.Configure(webHostBuilder =>
                {
                    if (_configureAppActions.Any())
                        _configureAppActions.ForEach(action => action.Invoke(webHostBuilder));
                    else
                    {
                        webHostBuilder.UseHttpsRedirection();
                        webHostBuilder.UseRouting();
                        webHostBuilder.UseAuthorization();
                        webHostBuilder.UseEndpoints(endpoints =>
                        {
                            endpoints.MapControllers();
                        });
                    }
                });
                if (_urls.Any())
                    webBuilder.UseUrls(_urls.ToArray());
            })
            .Build()
            .Run();
}
