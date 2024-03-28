using System;
using System.Collections.Generic;
using System.Linq;
using LethalAchievements.Helpers;
using LethalAchievements.Interfaces;
using LethalModDataLib.Features;

namespace LethalAchievements.Features;

/// <summary>
///     Registry for achievements.
/// </summary>
public class AchievementRegistry
{
    /// <summary>
    ///     Dictionary of all achievements. Key being the GUID of the achievement.
    /// </summary>
    private Dictionary<string, IAchievement> Achievements { get; } = new();

    /// <summary>
    ///     Get an achievement by its type.
    /// </summary>
    /// <typeparam name="T"> The type of the achievement to get. </typeparam>
    /// <returns> The achievement of type <typeparamref name="T" />. </returns>
    public T? GetAchievement<T>() where T : IAchievement, new()
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
    public T? GetAchievement<T>(string name) where T : IAchievement, new()
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
    internal bool AddAchievement(IAchievement achievement)
    {
        var achievementGuid = achievement.GetAchievementGuid();

        if (Achievements.ContainsKey(achievementGuid))
        {
            LethalAchievements.Logger?.LogWarning($"Achievement with guid \"{achievementGuid}\" already exists!");
            return false;
        }

        LethalAchievements.Logger?.LogDebug($"Adding achievement \"{achievementGuid}\"...");

        // We register any mod data attributes for the achievement
        ModDataHandler.RegisterInstance(achievement);
        
        // We load the IsAchieved state of the achievement from the save file
        achievement.LoadAchievedState();

        // We add the achievement to the dictionary
        Achievements.Add(achievementGuid, achievement);
        
        // If the achievement is not achieved, we initialize it
        if (!achievement.IsAchieved)
        {
            achievement.Initialize();
        }

        return true;
    }

    /// <summary>
    ///     Removes an achievement from the achievement dictionary.
    /// </summary>
    /// <param name="achievement"> The <see cref="IAchievement" /> to remove. </param>
    /// <returns> <see langword="true" /> if the achievement was removed successfully, <see langword="false" /> otherwise. </returns>
    internal bool RemoveAchievement(IAchievement achievement)
    {
        var achievementGuid = achievement.GetAchievementGuid();
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
}