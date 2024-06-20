namespace Aoxe.Shared;

public static partial class LoadHelper
{
    public static void FromAssemblyOf<T>() => FromAssemblyOf(typeof(T));

    public static void FromAssemblyOf<T0, T1>() => FromAssemblyOf(typeof(T0), typeof(T1));

    public static void FromAssemblyOf<T0, T1, T2>() =>
        FromAssemblyOf(typeof(T0), typeof(T1), typeof(T2));

    public static void FromAssemblyOf<T0, T1, T2, T3>() =>
        FromAssemblyOf(typeof(T0), typeof(T1), typeof(T2), typeof(T3));

    public static void FromAssemblyOf<T0, T1, T2, T3, T4>() =>
        FromAssemblyOf(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4));

    public static void FromAssemblyOf<T0, T1, T2, T3, T4, T5>() =>
        FromAssemblyOf(typeof(T0), typeof(T1), typeof(T2), typeof(T3), typeof(T4), typeof(T5));

    public static void FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6>() =>
        FromAssemblyOf(
            typeof(T0),
            typeof(T1),
            typeof(T2),
            typeof(T3),
            typeof(T4),
            typeof(T5),
            typeof(T6)
        );

    public static void FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7>() =>
        FromAssemblyOf(
            typeof(T0),
            typeof(T1),
            typeof(T2),
            typeof(T3),
            typeof(T4),
            typeof(T5),
            typeof(T6),
            typeof(T7)
        );

    public static void FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7, T8>() =>
        FromAssemblyOf(
            typeof(T0),
            typeof(T1),
            typeof(T2),
            typeof(T3),
            typeof(T4),
            typeof(T5),
            typeof(T6),
            typeof(T7),
            typeof(T8)
        );

    public static void FromAssemblyOf<T0, T1, T2, T3, T4, T5, T6, T7, T8, T9>() =>
        FromAssemblyOf(
            typeof(T0),
            typeof(T1),
            typeof(T2),
            typeof(T3),
            typeof(T4),
            typeof(T5),
            typeof(T6),
            typeof(T7),
            typeof(T8),
            typeof(T9)
        );

    public static void FromAssemblyOf(params Type[] types)
    {
        SpecifyTypes.AddRange(
            types
                .SelectMany(type => type.Assembly.GetTypes())
                .Where(p => !SpecifyTypes.Contains(p))
                .Distinct()
        );
        LoadMode = LoadTypesMode.LoadBySpecify;
    }
}
