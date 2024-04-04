using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using LethalAchievements.Enums;
using LethalAchievements.Features;
using LethalAchievements.UI;
using LethalModDataLib.Events;

namespace LethalAchievements;

/// <summary>
///     Main plugin class for LethalAchievements.
/// </summary>
[BepInDependency(LethalModDataLib.PluginInfo.PLUGIN_GUID)]
[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class LethalAchievements : BaseUnityPlugin
{
    internal new static ManualLogSource? Logger { get; private set; }
    
    internal static bool ArePluginsLoaded { get; private set; }
    
    internal static ConfigEntry<bool>? AchievementSoundEnabled { get; private set; }
    internal static ConfigEntry<AchievementPopupStyle>? AchievementPopupStyle { get; private set; }

    /// <summary>
    ///     Singleton instance of the plugin.
    /// </summary>
    public static LethalAchievements? Instance { get; private set; }

    private void Awake()
    {
        // Set instance
        Instance = this;

        // Init logger
        Logger = base.Logger;

        // Init config entries
        AchievementSoundEnabled = Config.Bind("General", "AchievementSoundEnabled", true,
            "Enable achievement sound effect when an achievement is completed.");
        AchievementPopupStyle = Config.Bind("General", "AchievementPopupStyle",
            Enums.AchievementPopupStyle.GlobalNotification, "The style of the achievement popup.");
        
        // Load UI and patch to main screen
        AchievementAssets.Load();
        new Harmony(PluginInfo.PLUGIN_GUID).PatchAll();
        
        
        // Hook into post game init event
        MiscEvents.PostInitializeGameEvent += OnGameLoaded;

        // Report plugin loaded
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private static void OnGameLoaded()
    {
        ArePluginsLoaded = true;
        
        // Initialize achievements system
        AchievementManager.Initialize();
    }
}