using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Zaaby.Core
{
    internal class ZaabyAppServiceControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly IEnumerable<Type> _serviceTypes;

        public ZaabyAppServiceControllerFeatureProvider(IEnumerable<Type> serviceTypes) =>
            _serviceTypes = serviceTypes;

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            foreach (var serviceType in _serviceTypes) feature.Controllers.Add(serviceType.GetTypeInfo());
        }
    }
}