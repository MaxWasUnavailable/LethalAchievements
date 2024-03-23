using LethalAchievements.Interfaces;

namespace LethalAchievements.Helpers;

public static class AchievementHelpers
{
    /// <summary>
    ///     Gets the GUID of the specified <see cref="IAchievement" />.
    /// </summary>
    /// <param name="achievement"> The <see cref="IAchievement" /> to get the GUID of. </param>
    /// <returns> The GUID of the specified <see cref="IAchievement" />. </returns>
    public static string GetAchievementGuid(IAchievement achievement)
    {
        return $"{achievement.GetType().Assembly.GetName().Name}.{achievement.Name}";
    }
}