namespace Zaaby.Server;

internal class ZaabyActionModelConvention : IActionModelConvention
{
    private readonly Type _serviceType;

    public ZaabyActionModelConvention(Type serviceType) => _serviceType = serviceType;

    public void Apply(ActionModel action)
    {
        if (action is null) throw new ArgumentNullException(nameof(action));
        if (!_serviceType.IsAssignableFrom(action.Controller.ControllerType)) return;

        action.Selectors.Clear();
        var route = _serviceType.GetCustomAttribute(typeof(RouteAttribute), false) as RouteAttribute;
        var template = $"{_serviceType.FullName?.Replace('.', '/')}/[action]";
        action.Selectors.Add(CreateSelector(route ?? new RouteAttribute(template) { Name = template }));

        foreach (var parameter in action.Parameters)
        {
            parameter.BindingInfo ??= new BindingInfo();
            parameter.BindingInfo.BindingSource = BindingSource.Body;
        }
    }

    private static SelectorModel CreateSelector(IRouteTemplateProvider routeTemplateProvider)
    {
        var selectorModel = new SelectorModel
        {
            AttributeRouteModel = new AttributeRouteModel(routeTemplateProvider)
        };

        selectorModel.ActionConstraints.Add(new HttpMethodActionConstraint(new List<string> { "POST" }));

        return selectorModel;
    }
}