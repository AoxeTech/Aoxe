using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Zaaby.Service
{
    internal class ZaabyAppServiceControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly IEnumerable<Type> _implementTypes;

        public ZaabyAppServiceControllerFeatureProvider(IEnumerable<Type> implementTypes) =>
            _implementTypes = implementTypes;

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            foreach (var serviceType in _implementTypes) feature.Controllers.Add(serviceType.GetTypeInfo());
        }
    }
}