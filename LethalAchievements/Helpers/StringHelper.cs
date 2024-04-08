using System.Linq;

namespace LethalAchievements.Helpers;

internal static class StringHelper
{
    public static string PascalToSnakeCase(string snakeCase)
    {
        return string.Concat(snakeCase.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString())).ToLower();
    }
}