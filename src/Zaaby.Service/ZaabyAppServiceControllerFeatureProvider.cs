using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Zaaby.Service
{
    internal class ZaabyAppServiceControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        private readonly List<Type> _implementTypes;

        public ZaabyAppServiceControllerFeatureProvider(List<Type> implementTypes) =>
            _implementTypes = implementTypes;

        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            _implementTypes.ForEach(serviceType => feature.Controllers.Add(serviceType.GetTypeInfo()));
        }
    }
}