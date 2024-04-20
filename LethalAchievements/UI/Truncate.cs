namespace LethalAchievements.UI;

internal static class StringExt
{
    internal static string? Truncate(this string? value, int maxLength)
    {
        return value?.Length > maxLength
            ? value.Substring(0, maxLength)
            : value;
    }
}