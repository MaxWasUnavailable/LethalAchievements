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
    ///     Get a dictionary of PluginInfo by achievements.
    /// </summary>
    /// <returns> A dictionary of PluginInfo by achievements. </returns>
    public Dictionary<IAchievement, BepInEx.PluginInfo> GetPluginsByAchievements()
    {
        return Achievements.Values.ToDictionary(achievement => achievement, achievement => achievement.GetPluginInfo());
    }
    
    /// <summary>
    ///     Get a dictionary of lists of achievements by their PluginInfo
    /// </summary>
    /// <returns> A dictionary of lists of achievements by their PluginInfo. </returns>
    public Dictionary<BepInEx.PluginInfo, List<IAchievement>> GetAchievementsByPlugins()
    {
        var achievementsByPlugins = new Dictionary<BepInEx.PluginInfo, List<IAchievement>>();
        
        // TODO: remove excessive debug logging
        LethalAchievements.Logger?.LogDebug("Getting achievements by plugins...");
        LethalAchievements.Logger?.LogDebug("Achievements count: " + Achievements.Count);
        
        foreach (var achievement in Achievements.Values)
        {
            LethalAchievements.Logger?.LogDebug($"Achievement: {achievement.Name}");
            var pluginInfo = achievement.GetPluginInfo();
            if (!achievementsByPlugins.ContainsKey(pluginInfo))
                achievementsByPlugins.Add(pluginInfo, []);
            
            achievementsByPlugins[pluginInfo].Add(achievement);
        }
        
        LethalAchievements.Logger?.LogDebug("Achievements by plugins count: " + achievementsByPlugins.Count);

        return achievementsByPlugins;
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

        // We add the achievement to the dictionary
        Achievements.Add(achievementGuid, achievement);

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
        
        // We uninitialize the achievement
        achievement.Uninitialize();
        
        // We remove the achievement from the dictionary
        Achievements.Remove(achievementGuid);
        
        // We deregister any mod data attributes for the achievement
        ModDataHandler.DeRegisterInstance(achievement);

        return true;
    }
    
    /// <summary>
    ///     Get all achievements.
    /// </summary>
    /// <returns> A list of all achievements. </returns>
    internal List<IAchievement> GetAchievements()
    {
        return Achievements.Values.ToList();
    }
}