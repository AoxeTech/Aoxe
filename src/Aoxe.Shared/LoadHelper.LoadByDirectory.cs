namespace Zaaby.Shared;

public static partial class LoadHelper
{
    public static void FromDirectories(params string[] directories)
    {
        SpecifyTypes.AddRange(LoadFromDirectories(directories));
        LoadMode = LoadTypesMode.LoadBySpecify;
    }

    private static List<Type> LoadFromDirectories(params string[] directories)
    {
        var files = directories.SelectMany(dir => Directory
            .GetFiles(dir + @"/", "*.dll", SearchOption.AllDirectories)
            .Union(Directory.GetFiles(dir + @"/", "*.exe", SearchOption.AllDirectories)));

        var result = files.Select(file =>
            {
                try
                {
                    return Assembly.LoadFrom(file).ExportedTypes;
                }
                catch (BadImageFormatException)
                {
                    return null;
                }
                catch (FileLoadException)
                {
                    return null;
                }
                catch (Exception)
                {
                    return null;
                }
            })
            .Where(types => types is not null)
            .SelectMany(types => types.Where(g => g is not null))
            .Distinct()
            .ToList();

        LoadMode = LoadTypesMode.LoadByAllDirectory;
        return result;
    }
}