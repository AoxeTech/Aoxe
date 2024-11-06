namespace Aoxe.WebApi;

internal class AoxeAppServiceControllerFeatureProvider
    : IApplicationFeatureProvider<ControllerFeature>
{
    private readonly List<Type> _implementTypes;

    public AoxeAppServiceControllerFeatureProvider(List<Type> implementTypes) =>
        _implementTypes = implementTypes;

    public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature) =>
        _implementTypes.ForEach(serviceType => feature.Controllers.Add(serviceType.GetTypeInfo()));
}
