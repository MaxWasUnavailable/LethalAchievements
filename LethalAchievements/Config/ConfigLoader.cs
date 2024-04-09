using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace LethalAchievements.Config;

public static class ConfigLoader
{
    public static IEnumerable<ConfigAchievement?> LoadAchievements(string root)
    {
        return Directory.EnumerateDirectories(root, "Achievements", SearchOption.AllDirectories)
            .SelectMany(directory => Directory.EnumerateFiles(directory, "*.json"))
            .Select(file => {
                var json = File.ReadAllText(file);
                try
                {
                    var configFile = Json.Deserialize<ConfigAchievementFile>(json);
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