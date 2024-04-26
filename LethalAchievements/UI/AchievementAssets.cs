using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace LethalAchievements.UI;

internal static class AchievementAssets
{
    private static AssetBundle? _assets;
    internal static GameObject? UIAssets;

    internal static bool Load()
    {
        var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "achievement_assets");

        if (!File.Exists(path))
        {
            LethalAchievements.Logger?.LogWarning("Failed to find achievement assets!");
            return false;
        }

        _assets = AssetBundle.LoadFromFile(path);

        if (_assets == null)
        {
            LethalAchievements.Logger?.LogWarning("Failed to load achievement assets!");
            return false;
        }

        List<bool> loadResults =
        [
            LoadFile(_assets, "Assets/UI.prefab", out UIAssets)
        ];

        if (loadResults.All(result => result))
            return true;

        LethalAchievements.Logger?.LogWarning("Failed to load one or more assets from ./achievement_assets!");
        return false;
    }

    private static bool LoadFile<T>(AssetBundle? assets, string path, out T? loadedObject) where T : Object?
    {
        loadedObject = assets?.LoadAsset<T>(path);
        if (loadedObject)
            return true;

        LethalAchievements.Logger?.LogWarning($"Failed to load '{path}' from ./achievement_assets");
        return false;
    }
}