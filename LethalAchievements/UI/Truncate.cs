namespace LethalAchievements.UI;

internal static class StringExt
{
    internal static string? Truncate(this string? value, int maxLength)
    {
        return value?.Length > maxLength
            ? value[..maxLength]
            : value;
    }
}