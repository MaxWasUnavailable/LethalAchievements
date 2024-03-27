using System;
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
    /// <summary>
    ///     Dictionary of all achievements. Key being the GUID of the achievement.
    /// </summary>
    private static Dictionary<string, IAchievement> Achievements { get; } = new();
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
        foreach (var achievement in AchievementsToAdd)
        {
            AddAchievement(achievement);
        }
    }

    /// <summary>
    ///     Get an achievement by its type.
    /// </summary>
    /// <typeparam name="T"> The type of the achievement to get. </typeparam>
    /// <returns> The achievement of type <typeparamref name="T" />. </returns>
    public static T? GetAchievement<T>() where T : IAchievement, new()
    {
        var achievement = Achievements.Values.FirstOrDefault(achievement => achievement.GetType() == typeof(T));
        return achievement != null ? (T)achievement : default;
    }

    /// <summary>
    ///     Get an achievement by its type and name.
    /// </summary>
    /// <param name="name"> The name of the achievement to get. </param>
    /// <typeparam name="T"> The type of the achievement to get. </typeparam>
    /// <returns> The achievement of type <typeparamref name="T" /> with the name <paramref name="name" />. </returns>
    /// <remarks> Might be redundant, but made available in case it might be useful. </remarks>
    public static T? GetAchievement<T>(string name) where T : IAchievement, new()
    {
        var achievement = Achievements.Values.FirstOrDefault(achievement =>
            achievement.GetType() == typeof(T) &&
            string.Equals(achievement.Name, name, StringComparison.CurrentCultureIgnoreCase));
        return achievement != null ? (T)achievement : default;
    }

    /// <summary>
    ///     Adds an achievement to the achievement dictionary.
    /// </summary>
    /// <param name="achievement"> The <see cref="IAchievement" /> to add. </param>
    /// <returns> <see langword="true" /> if the achievement was added successfully, <see langword="false" /> otherwise. </returns>
    internal static bool AddAchievement(IAchievement achievement)
    {
        var achievementGuid = AchievementHelper.GetAchievementGuid(achievement);

        if (Achievements.ContainsKey(achievementGuid))
        {
            LethalAchievements.Logger?.LogWarning($"Achievement with guid \"{achievementGuid}\" already exists!");
            return false;
        }

        LethalAchievements.Logger?.LogDebug($"Adding achievement \"{achievementGuid}\"...");

        achievement.Initialize();
        Achievements.Add(achievementGuid, achievement);
        achievement.AchievedEvent += () => OnAchieved(achievement);

        return true;
    }

    /// <summary>
    ///     Removes an achievement from the achievement dictionary.
    /// </summary>
    /// <param name="achievement"> The <see cref="IAchievement" /> to remove. </param>
    /// <returns> <see langword="true" /> if the achievement was removed successfully, <see langword="false" /> otherwise. </returns>
    internal static bool RemoveAchievement(IAchievement achievement)
    {
        var achievementGuid = AchievementHelper.GetAchievementGuid(achievement);
        if (!Achievements.ContainsKey(achievementGuid))
        {
            LethalAchievements.Logger?.LogWarning($"Achievement with guid \"{achievementGuid}\" does not exist!");
            return false;
        }

        LethalAchievements.Logger?.LogDebug($"Removing achievement \"{achievementGuid}\"...");

        achievement.Uninitialize();
        Achievements.Remove(achievementGuid);

        return true;
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

    // TODO: UI for achievements
    // TODO: Achievement save/load
}