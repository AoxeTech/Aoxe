using System;
using System.Collections.Generic;
using System.Linq;

namespace Zaaby.Common
{
    public static partial class LoadHelper
    {
        public static List<TypePair> GetByBaseType<T>() => GetByBaseType(typeof(T));

        public static List<TypePair> GetByBaseType(Type baseType)
        {
            var types = LoadTypes()
                .Where(baseType.IsAssignableFrom)
                .ToList();

            var interfaceTypes = types.Where(type => type.IsInterface && type != baseType).ToList();
            var implementationTypes = types.Where(type => type.IsClass).ToList();

            return CreateTypePairs(interfaceTypes, implementationTypes);
        }

        public static List<TypePair> GetByAttribute<TAttribute>() where TAttribute : Attribute =>
            GetByAttribute(typeof(TAttribute));

        public static List<TypePair> GetByAttribute(Type attributeType)
        {
            var types = LoadTypes()
                .Where(type => Attribute.GetCustomAttribute(type, attributeType, true) is not null)
                .ToList();

            var interfaceTypes = types.Where(type => type.IsInterface).ToList();
            var implementationTypes = types.Where(type => type.IsClass).ToList();

            return CreateTypePairs(interfaceTypes, implementationTypes);
        }

        private static List<TypePair> CreateTypePairs(IEnumerable<Type> interfaceTypes,
            IEnumerable<Type> implementationTypes)
        {
            var result = interfaceTypes
                .Select(interfaceType => new TypePair(interfaceType,
                    implementationTypes.FirstOrDefault(interfaceType.IsAssignableFrom))).ToList();

            result.AddRange(implementationTypes.Where(c => !result.Select(r => r.ImplementationType).Contains(c))
                .Select(implementationType => new TypePair(null, implementationType)));

            return result;
        }
    }
}