using System;
using System.Collections.Generic;

namespace Zaaby.Common
{
    public static partial class LoadHelper
    {
        public static LoadMode LoadMode { get; set; } = LoadMode.LoadAll;

        public static IReadOnlyList<Type> LoadTypes() =>
            LoadMode is LoadMode.LoadByScan
                ? LoadScanTypes()
                : LoadAllTypes();
    }
}