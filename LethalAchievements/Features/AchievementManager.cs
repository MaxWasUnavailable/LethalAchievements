using System.Collections.Generic;
using System.Linq;
using LethalAchievements.Helpers;
using LethalAchievements.Interfaces;

namespace LethalAchievements.Features;

/// <summary>
///     Manager for achievements.
/// </summary>
public static class AchievementManager
{
    private static AchievementRegistry AchievementRegistry { get; } = new();
    private static List<IAchievement> AchievementsToAdd { get; } = [];

    /// <summary>
    ///     Registers an achievement with the achievement manager.
    /// </summary>
    /// <param name="achievement"> The <see cref="IAchievement" /> to register. </param>
    public static void RegisterAchievement(IAchievement achievement)
    {
        LethalAchievements.Logger?.LogDebug(
            $"Registering achievement \"{achievement.Name}\" from \"{achievement.GetType().Assembly.FullName}\"...");
        AchievementsToAdd.Add(achievement);
    }
    
    /// <summary>
    ///     Initializes all achievements.
    ///     Done after the ChainLoader has loaded all plugins, since otherwise GUID of plugins is not available.
    /// </summary>
    internal static void InitializeAchievements()
    {
        foreach (var achievement in AchievementsToAdd.Where(achievement => AchievementRegistry.AddAchievement(achievement)))
        {
            achievement.AchievedEvent += () => OnAchieved(achievement);
        }
    }

    /// <summary>
    ///     Called when an achievement is achieved. Used to handle achievement events.
    /// </summary>
    /// <param name="achievement"> The <see cref="IAchievement" /> that was achieved. </param>
    private static void OnAchieved(IAchievement achievement)
    {
        SoundHelper.PlayLevelUpSound();
        AchievementHelper.AchievementChatMessage(achievement);
        AchievementHelper.DisplayAchievementAsStatus(achievement);
        AchievementHelper.DisplayAchievementAsTip(achievement);

        achievement.Uninitialize();

        LethalAchievements.Logger?.LogDebug($"Achievement \"{achievement.Name}\" achieved!");
    }
}