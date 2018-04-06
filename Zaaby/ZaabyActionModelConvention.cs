using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Zaaby
{
    internal class ZaabyActionModelConvention : IActionModelConvention
    {
        private Type ServiceType { get; }

        public ZaabyActionModelConvention(Type serviceType)
        {
            ServiceType = serviceType;
        }

        public void Apply(ActionModel action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            if (!ServiceType.IsAssignableFrom(action.Controller.ControllerType)) return;

            action.Selectors.Clear();
            var template = $"{ServiceType.FullName.Replace('.','/')}/[action]";
            action.Selectors.Add(CreateSelector(new RouteAttribute(template) {Name = template}));

            foreach (var parameter in action.Parameters)
            {
                parameter.BindingInfo = parameter.BindingInfo ?? new BindingInfo();
                parameter.BindingInfo.BindingSource = BindingSource.Body;
            }
        }

        private static SelectorModel CreateSelector(IRouteTemplateProvider routeTemplateProvider)
        {
            var selectorModel = new SelectorModel();
            if (routeTemplateProvider != null)
                selectorModel.AttributeRouteModel = new AttributeRouteModel(routeTemplateProvider);

            selectorModel.ActionConstraints.Add(new HttpMethodActionConstraint(new List<string> {"GET", "POST"}));

            return selectorModel;
        }
    }
}