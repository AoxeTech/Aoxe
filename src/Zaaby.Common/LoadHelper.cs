using System;
using System.Collections.Generic;
using System.Linq;

namespace Zaaby.Common
{
    public static class LoadHelper
    {
        private static IEnumerable<Type> _types;
        private static readonly object LockObj = new();

        public static void Scan(params Type[] types)
        {
            _types = types.SelectMany(type => type.Assembly.ExportedTypes).Distinct();
        }

        public static IEnumerable<Type> GetAllTypes()
        {
            if (_types != null) return _types;
            lock (LockObj)
            {
                _types = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(assembly => assembly.ExportedTypes).Distinct();
            }

            return _types;
        }
    }
}