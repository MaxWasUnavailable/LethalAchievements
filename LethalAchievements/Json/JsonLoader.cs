using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LethalAchievements.Config;

/// <summary>
///     Utility for loading achievements from .json files.
/// </summary>
public static class JsonLoader
{
    /// <summary>
    ///     Searches <paramref name="root"/> and subdirectories for directories named "Achievements",
    ///     and loads all .json files in those directories as achievements. Does not throw when an achievement
    ///     fails to load, but instead logs an error yields null.
    /// </summary>
    public static IEnumerable<JsonAchievement?> LoadAchievements(string root)
    {
        return Directory.EnumerateDirectories(root, "Achievements", SearchOption.AllDirectories)
            .SelectMany(directory => Directory.EnumerateFiles(directory, "*.json"))
            .Select(file => {
                var json = File.ReadAllText(file);
                try
                {
                    var configFile = Json.Deserialize<JsonAchievementFile>(json);
                    return configFile!.ToAchievement(file);
                }
                catch (Exception e)
                {
                    LethalAchievements.Logger!.LogError($"Failed to load achievement from {file}: {e}");
                    return null;
                }
            });
    }
}