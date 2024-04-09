using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LethalAchievements.Config;

/// <summary>
///     Utility for finding and loading achievement JSON files.
/// </summary>
public static class JsonAchievementLoader
{
    /// <summary>
    ///     Searches root and its subdirectories for 'Achievements' directories, then loads all JSON files in them.
    ///     This does not throw exceptions for individual files that fail to load, but instead yields null.
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