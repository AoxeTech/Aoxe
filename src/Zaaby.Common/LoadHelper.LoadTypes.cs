using System;
using System.Collections.Generic;

namespace Zaaby.Common
{
    public static partial class LoadHelper
    {
        public static LoadTypesMode LoadMode { get; set; } = LoadTypesMode.LoadByDirectory;

        public static IReadOnlyList<Type> LoadTypes() =>
            LoadMode switch
            {
                LoadTypesMode.LoadByScan => LoadScanTypes(),
                LoadTypesMode.LoadByAssemblies => LoadAssemblyTypes(),
                _ => LoadDirectoryTypes()
            };
    }
}