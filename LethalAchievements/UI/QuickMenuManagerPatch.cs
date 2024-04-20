using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace LethalAchievements.UI;

internal class QuickMenuManagerPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(QuickMenuManager), nameof(QuickMenuManager.OpenQuickMenu))]
    [HarmonyPatch(typeof(QuickMenuManager), nameof(QuickMenuManager.CloseQuickMenuPanels))]
    private static void OpenUI()
    {
        LethalAchievements.Logger?.LogInfo("Updating UI");
        
        
        foreach (var mod in HUDController.ModList)
        {
            foreach (var achievement in mod.AchievementEntries)
            {
                achievement.Value.UpdateProgress(achievement.Key);
            }
        }
        LethalAchievements.UI.gameObject.SetActive(true);
    }
    [HarmonyPostfix]
    [HarmonyPatch(typeof(QuickMenuManager), nameof(QuickMenuManager.EnableUIPanel))]
    [HarmonyPatch(typeof(QuickMenuManager), nameof(QuickMenuManager.CloseQuickMenu))]
    [HarmonyPatch(typeof(QuickMenuManager), nameof(QuickMenuManager.LeaveGame))]
    
    private static void CloseUI()
    {
        LethalAchievements.Logger?.LogInfo("Closing UI");
        
        LethalAchievements.UI.gameObject.SetActive(false);
    }
}