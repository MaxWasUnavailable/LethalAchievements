namespace LethalAchievements.Helpers;

/// <summary>
///    Helper class for string-related features.
/// </summary>
public static class StringHelper
{
    /// <summary>
    ///     Converts a string from PascalCase to camelCase.
    /// </summary>
    public static string ToCamelCase(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return input;
        }

        return char.ToLower(input[0]) + input.Substring(1);
    }
}