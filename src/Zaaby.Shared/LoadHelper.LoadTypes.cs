namespace Zaaby.Shared;

public static partial class LoadHelper
{
    private static readonly List<Type> SpecifyTypes = new();

    public static LoadTypesMode LoadMode { get; set; } = LoadTypesMode.LoadByAllDirectory;

    public static IReadOnlyList<Type> LoadAllDirectoryTypes() => DirectoryTypesLazy.Value;

    public static IReadOnlyList<Type> LoadSpecifyTypes() => SpecifyTypes;

    public static IReadOnlyList<Type> LoadTypes() =>
        LoadMode switch
        {
            LoadTypesMode.LoadBySpecify => LoadSpecifyTypes(),
            _ => LoadAllDirectoryTypes()
        };

    private static readonly Lazy<List<Type>> DirectoryTypesLazy = new(() =>
    {
        var result = LoadFromDirectories(Directory.GetCurrentDirectory());
        LoadMode = LoadTypesMode.LoadByAllDirectory;
        return result;
    });
}