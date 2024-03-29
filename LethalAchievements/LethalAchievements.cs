using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using LethalAchievements.Achievements;
using LethalAchievements.Enums;
using LethalAchievements.Features;
using LethalModDataLib.Events;

namespace LethalAchievements;

/// <summary>
///     Main plugin class for LethalAchievements.
/// </summary>
[BepInDependency(LethalEventsLib.PluginInfo.PLUGIN_GUID)]
[BepInDependency(LethalModDataLib.PluginInfo.PLUGIN_GUID)]
[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class LethalAchievements : BaseUnityPlugin
{
    internal new static ManualLogSource? Logger { get; private set; }

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

        // Register example achievements
        // TODO: remove for release
        AchievementManager.RegisterAchievement(new JumpAchievement());
        AchievementManager.RegisterAchievement(new Jump10Achievement());
        AchievementManager.RegisterAchievement(new GeneralJumpAchievement());

        // Hook into post game init event
        MiscEvents.PostInitializeGameEvent += OnGameLoaded;

        // Report plugin loaded
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private static void OnGameLoaded()
    {
        // Initialize achievements system
        AchievementManager.Initialize();
    }
}