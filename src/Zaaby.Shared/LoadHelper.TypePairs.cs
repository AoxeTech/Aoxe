using System;
using System.Collections.Generic;
using System.Linq;

namespace Zaaby.Shared
{
    public static partial class LoadHelper
    {
        public static List<TypePair> GetTypePairs(params Type[] defineTypes)
        {
            var types = defineTypes
                .SelectMany(defineType => typeof(Attribute).IsAssignableFrom(defineType)
                    ? GetByAttributes(defineType)
                    : GetByBaseTypes(defineType))
                .Distinct()
                .ToList();

            var interfaceTypes = types.Where(type => type.IsInterface && !defineTypes.Contains(type));
            var implementationTypes = types.Where(type => type.IsClass);

            return CreateTypePairs(interfaceTypes, implementationTypes);
        }

        public static List<Type> GetByBaseTypes(params Type[] baseTypes) =>
            LoadTypes().Where(p => baseTypes.Any(baseType => baseType.IsAssignableFrom(p)))
                .Distinct()
                .ToList();

        public static List<Type> GetByAttributes(params Type[] attributeTypes) =>
            LoadTypes().Where(type => attributeTypes.Any(attributeType =>
                    Attribute.GetCustomAttribute(type, attributeType, true) is not null))
                .Distinct()
                .ToList();

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