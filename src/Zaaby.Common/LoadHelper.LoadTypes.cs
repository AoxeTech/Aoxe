using System;
using System.Collections.Generic;

namespace Zaaby.Common
{
    public static partial class LoadHelper
    {
        private static readonly List<Type> SpecifyTypes = new();

        public static LoadTypesMode LoadMode { get; set; } = LoadTypesMode.LoadByDirectory;

        public static IReadOnlyList<Type> LoadDirectoryTypes() => DirectoryTypesLazy.Value;

        public static IReadOnlyList<Type> LoadSpecifyTypes() => SpecifyTypes;

        public static IReadOnlyList<Type> LoadTypes() =>
            LoadMode switch
            {
                LoadTypesMode.LoadBySpecify => LoadSpecifyTypes(),
                _ => LoadDirectoryTypes()
            };
    }
}