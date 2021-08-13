using System;
using System.Collections.Generic;
using System.Linq;

namespace Zaaby.Common
{
    public static partial class LoadHelper
    {
        public static List<TypePair> GetByAttribute<TAttribute>()
            where TAttribute : Attribute =>
            GetByAttributes(typeof(TAttribute));

        public static List<TypePair> GetByAttribute<T0, T1>()
            where T0 : Attribute
            where T1 : Attribute =>
            GetByAttributes(typeof(T0), typeof(T1));

        public static List<TypePair> GetByAttribute<T0, T1, T2>()
            where T0 : Attribute
            where T1 : Attribute
            where T2 : Attribute =>
            GetByAttributes(typeof(T0), typeof(T1), typeof(T2));

        public static List<TypePair> GetByAttribute<T0, T1, T2, T3>()
            where T0 : Attribute
            where T1 : Attribute
            where T2 : Attribute
            where T3 : Attribute =>
            GetByAttributes(typeof(T0), typeof(T1), typeof(T2), typeof(T3));

        public static List<TypePair> GetByAttribute<T0, T1, T2, T3, T4>()
            where T0 : Attribute
            where T1 : Attribute
            where T2 : Attribute
            where T3 : Attribute
            where T4 : Attribute =>
            GetByAttributes(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4));

        public static List<TypePair> GetByAttribute<T0, T1, T2, T3, T4, T5>()
            where T0 : Attribute
            where T1 : Attribute
            where T2 : Attribute
            where T3 : Attribute
            where T4 : Attribute
            where T5 : Attribute =>
            GetByAttributes(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));

        public static List<TypePair> GetByAttribute<T0, T1, T2, T3, T4, T5, T6>()
            where T0 : Attribute
            where T1 : Attribute
            where T2 : Attribute
            where T3 : Attribute
            where T4 : Attribute
            where T5 : Attribute
            where T6 : Attribute =>
            GetByAttributes(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6));

        public static List<TypePair> GetByAttribute<T0, T1, T2, T3, T4, T5, T6, T7>()
            where T0 : Attribute
            where T1 : Attribute
            where T2 : Attribute
            where T3 : Attribute
            where T4 : Attribute
            where T5 : Attribute
            where T6 : Attribute
            where T7 : Attribute =>
            GetByAttributes(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6),
                typeof(T7));

        public static List<TypePair> GetByAttribute<T0, T1, T2, T3, T4, T5, T6, T7, T8>()
            where T0 : Attribute
            where T1 : Attribute
            where T2 : Attribute
            where T3 : Attribute
            where T4 : Attribute
            where T5 : Attribute
            where T6 : Attribute
            where T7 : Attribute
            where T8 : Attribute =>
            GetByAttributes(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6),
                typeof(T7), typeof(T8));

        public static List<TypePair> GetByAttribute<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>()
            where T0 : Attribute
            where T1 : Attribute
            where T2 : Attribute
            where T3 : Attribute
            where T4 : Attribute
            where T5 : Attribute
            where T6 : Attribute
            where T7 : Attribute
            where T8 : Attribute
            where T9 : Attribute =>
            GetByAttributes(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5), typeof(T6),
                typeof(T7), typeof(T8), typeof(T9));

        public static List<TypePair> GetByAttributes(params Type[] attributeTypes)
        {
            var types = LoadTypes()
                .Where(type => attributeTypes.Any(attributeType =>
                    Attribute.GetCustomAttribute(type, attributeType, true) is not null))
                .ToList();

            var interfaceTypes = types.Where(type => type.IsInterface);
            var implementationTypes = types.Where(type => type.IsClass);

            return CreateTypePairs(interfaceTypes, implementationTypes);
        }
    }
}