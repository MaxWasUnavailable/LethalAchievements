using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace LethalAchievements.UI;

[HarmonyPatch(typeof(MenuManager))]
public class MenuManagerPatch
{
    [HarmonyPatch("Start"), HarmonyPostfix]
    private static void LoadUI()
    {
        LethalAchievements.Logger?.LogInfo("Loading UI...");
        
        Object.Instantiate(AchievementAssets.UIAssets).AddComponent<HUDController>();
    }
}