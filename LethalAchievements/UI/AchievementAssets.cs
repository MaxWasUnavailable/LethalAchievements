using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace LethalAchievements.UI;

internal static class AchievementAssets
{
    private static AssetBundle Assets;
    internal static GameObject UIAssets;
    internal static bool Load()
    {
        Assets = AssetBundle.LoadFromFile(
            Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "achievement_assets"));
        if (Assets == null)
        {
            LethalAchievements.Logger?.LogWarning("Failed to load achievement assets, aborting!");
            return false;
        }

        List<bool> loadResults =
        [
            LoadFile(Assets, "Assets/UI.prefab", out UIAssets),
        ];
        if (loadResults.Any(result => result == false))
        {
            LethalAchievements.Logger?.LogWarning("Failed to load one or more assets from ./achievement_assets, aborting!");
            return false;
        }
        return true;
    }
    
    private static bool LoadFile<T>(AssetBundle assets, string path, out T loadedObject) where T : Object
    {
        loadedObject = assets.LoadAsset<T>(path);
        if (!loadedObject)
        {
            LethalAchievements.Logger?.LogError($"Failed to load '{path}' from ./achievement_assets");
            return false;
        }
        
        return true;
    }
}