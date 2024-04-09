using System.Linq;

namespace LethalAchievements.Helpers;

internal static class StringHelper
{
    public static string PascalToSnakeCase(string snakeCase)
    {
        return string.Concat(snakeCase.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString())).ToLower();
    }

    public static string SnakeToPascalCase(string pascalCase)
    {
        return string.Concat(pascalCase.Split('_').Select(x => char.ToUpper(x[0]) + x.Substring(1).ToLower()));
    }
}