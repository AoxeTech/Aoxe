using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Zaaby.Service
{
    internal class ZaabyActionModelConvention : IActionModelConvention
    {
        private readonly Type _interfaceType;

        public ZaabyActionModelConvention(Type interfaceType) => _interfaceType = interfaceType;

        public void Apply(ActionModel action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));
            if (!_interfaceType.IsAssignableFrom(action.Controller.ControllerType)) return;

            action.Selectors.Clear();
            var template = $"{_interfaceType.FullName?.Replace('.', '/')}/[action]";
            action.Selectors.Add(CreateSelector(new RouteAttribute(template) {Name = template}));

            foreach (var parameter in action.Parameters)
            {
                parameter.BindingInfo ??= new BindingInfo();
                parameter.BindingInfo.BindingSource = BindingSource.Body;
            }
        }

        private static SelectorModel CreateSelector(IRouteTemplateProvider routeTemplateProvider)
        {
            var selectorModel = new SelectorModel();
            if (routeTemplateProvider is not null)
                selectorModel.AttributeRouteModel = new AttributeRouteModel(routeTemplateProvider);

            selectorModel.ActionConstraints.Add(new HttpMethodActionConstraint(new List<string> {"POST"}));

            return selectorModel;
        }
    }
}