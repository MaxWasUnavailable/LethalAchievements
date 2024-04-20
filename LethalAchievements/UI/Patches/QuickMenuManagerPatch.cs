using System.Linq;
using HarmonyLib;

namespace LethalAchievements.UI.Patches;

/// <summary>
///     Patches the QuickMenuManager to enable opening and closing the UI
/// </summary>
[HarmonyPatch(typeof(QuickMenuManager))]
internal static class QuickMenuManagerPatch
{
    /// <summary>
    ///     Updates and enables the UI when the QuickMenu is opened
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(QuickMenuManager.OpenQuickMenu))]
    [HarmonyPatch(nameof(QuickMenuManager.CloseQuickMenuPanels))]
    private static void OpenUI()
    {
        LethalAchievements.Logger?.LogDebug("Updating UI");

        foreach (var achievement in HUDController.ModList.SelectMany(mod => mod.AchievementEntries))
            achievement.Value.UpdateProgress(achievement.Key);

        HUDController.Instance!.gameObject.SetActive(true);
    }

    /// <summary>
    ///     Closes the UI when the QuickMenu is closed
    /// </summary>
    [HarmonyPostfix]
    [HarmonyPatch(nameof(QuickMenuManager.EnableUIPanel))]
    [HarmonyPatch(nameof(QuickMenuManager.CloseQuickMenu))]
    [HarmonyPatch(nameof(QuickMenuManager.LeaveGame))]
    private static void CloseUI()
    {
        LethalAchievements.Logger?.LogDebug("Closing UI");
        
        HUDController.Instance?.CloseUI();

        HUDController.Instance!.gameObject.SetActive(false);
    }
}