namespace Zaaby.Client.Http.Internal;

internal static class StringExtensions
{
    internal static string TrimEnd(this string target, string trimString)
    {
        if (string.IsNullOrEmpty(trimString)) return target;

        var result = target;
        while (result.EndsWith(trimString))
            result = result[..^trimString.Length];

        return result;
    }
}