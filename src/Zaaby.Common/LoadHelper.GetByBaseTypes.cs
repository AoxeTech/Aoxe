using System;
using System.Collections.Generic;
using System.Linq;

namespace Zaaby.Common
{
    public static partial class LoadHelper
    {
        public static List<TypePair> GetByBaseType<T>() => GetByBaseTypes(typeof(T));
        public static List<TypePair> GetByBaseType<T0, T1>() => GetByBaseTypes(typeof(T0), typeof(T1));
        public static List<TypePair> GetByBaseType<T0, T1, T2>() => GetByBaseTypes(typeof(T0), typeof(T1), typeof(T2));

        public static List<TypePair> GetByBaseType<T0, T1, T2, T3>() =>
            GetByBaseTypes(typeof(T0), typeof(T1), typeof(T2), typeof(T3));

        public static List<TypePair> GetByBaseType<T0, T1, T2, T3, T4>() =>
            GetByBaseTypes(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4));

        public static List<TypePair> GetByBaseType<T0, T1, T2, T3, T4, T5>() => GetByBaseTypes(typeof(T0), typeof(T1),
            typeof(T2), typeof(T3), typeof(T4), typeof(T5));

        public static List<TypePair> GetByBaseType<T0, T1, T2, T3, T4, T5, T6>() => GetByBaseTypes(typeof(T0),
            typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));

        public static List<TypePair> GetByBaseType<T0, T1, T2, T3, T4, T5, T6, T7>() => GetByBaseTypes(typeof(T0),
            typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7));

        public static List<TypePair> GetByBaseType<T0, T1, T2, T3, T4, T5, T6, T7, T8>() => GetByBaseTypes(typeof(T0),
            typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8));

        public static List<TypePair> GetByBaseType<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>() => GetByBaseTypes(
            typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6), typeof(T7), typeof(T8),
            typeof(T9));

        public static List<TypePair> GetByBaseTypes(params Type[] baseTypes)
        {
            var types = LoadTypes()
                .Where(p => baseTypes.Any(baseType => baseType.IsAssignableFrom(p)))
                .ToList();

            var interfaceTypes = types.Where(type => type.IsInterface && !baseTypes.Contains(type));
            var implementationTypes = types.Where(type => type.IsClass);

            return CreateTypePairs(interfaceTypes, implementationTypes);
        }
    }
}