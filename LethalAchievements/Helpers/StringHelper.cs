using System.Linq;

namespace LethalAchievements.Helpers;

/// <summary>
///     Helper class for string operations.
/// </summary>
public static class StringHelper
{
    public static string PascalToSnakeCase(string snakeCase)
    {
        return string.Concat(snakeCase.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString())).ToLower();
    }

    internal static string Truncate(this string value, int maxLength)
    {
        return value.Length > maxLength
            ? value[..maxLength]
            : value;
    }
}