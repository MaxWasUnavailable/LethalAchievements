using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace LethalAchievements.UI;

internal class QuickMenuManagerPatch
{
    private static HUDController UI;

    [HarmonyPostfix]
    [HarmonyPatch(typeof(MenuManager), "Awake")]
    private static void InitUI()
    {
        if (UI != null) return;
        LethalAchievements.Logger?.LogInfo("Initialising achievements UI");
        
        UI = Object.Instantiate(AchievementAssets.UIAssets).AddComponent<HUDController>();
        UI.hideFlags = HideFlags.HideAndDontSave;
        Object.DontDestroyOnLoad(UI.gameObject);
        
        // Create mod tabs and a list of achievements for each
        UI.InitializeUI();
        UI.gameObject.SetActive(false);
        
    }
    
    [HarmonyPostfix]
    [HarmonyPatch(typeof(QuickMenuManager), nameof(QuickMenuManager.OpenQuickMenu))]
    [HarmonyPatch(typeof(QuickMenuManager), nameof(QuickMenuManager.CloseQuickMenuPanels))]
    private static void OpenUI()
    {
        LethalAchievements.jumpAchievement.OnPlayerJump();
        
        LethalAchievements.Logger?.LogInfo("Updating UI");
        
        foreach (var mod in HUDController.ModList)
        {
            foreach (var achievement in mod.AchievementEntries)
            {
                achievement.Value.UpdateProgress(achievement.Key);
            }
        }
        LethalAchievements.Logger?.LogInfo($"{UI} REALL");
        UI.gameObject.SetActive(true);
    }
    [HarmonyPostfix]
    [HarmonyPatch(typeof(QuickMenuManager), nameof(QuickMenuManager.EnableUIPanel))]
    [HarmonyPatch(typeof(QuickMenuManager), nameof(QuickMenuManager.CloseQuickMenu))]
    [HarmonyPatch(typeof(QuickMenuManager), nameof(QuickMenuManager.LeaveGame))]
    
    private static void CloseUI()
    {
        LethalAchievements.Logger?.LogInfo("Closing UI");
        
        UI.gameObject.SetActive(false);
    }
}