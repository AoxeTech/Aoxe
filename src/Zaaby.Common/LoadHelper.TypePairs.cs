using System;
using System.Collections.Generic;
using System.Linq;

namespace Zaaby.Common
{
    public static partial class LoadHelper
    {
        private static List<TypePair> CreateTypePairs(IEnumerable<Type> interfaceTypes,
            IEnumerable<Type> implementationTypes)
        {
            var result = interfaceTypes.Select(interfaceType =>
                    new TypePair(interfaceType, implementationTypes.FirstOrDefault(interfaceType.IsAssignableFrom)))
                .ToList();

            result.AddRange(implementationTypes
                .Where(c => result.All(r => r.ImplementationType != c))
                .Select(implementationType => new TypePair(null, implementationType)));

            return result;
        }
    }
}