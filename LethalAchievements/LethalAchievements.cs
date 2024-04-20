using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using LethalAchievements.Config;
using LethalAchievements.Enums;
using LethalAchievements.Events;
using LethalAchievements.Events.Patches;
using LethalAchievements.Features;
using LethalAchievements.Patches;
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

        // Hook into post game init event
        MiscEvents.PostInitializeGameEvent += OnGameLoaded;

        // Run patches
        // should maybe find some more maintainable way to do this
        var harmony = new Harmony(PluginInfo.PLUGIN_GUID);
        harmony.PatchAll(typeof(PlayerEvents.Patches));
        harmony.PatchAll(typeof(QuickMenuManagerPatch));

        EnemyDamageSource.Patch(harmony);

        // Report plugin loaded
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private static void OnGameLoaded()
    {
        ArePluginsLoaded = true;

        // Load json achievements
        foreach (var achievement in JsonLoader.LoadAchievements(Paths.PluginPath))
        {
            if (achievement == null) continue;

            Logger!.LogDebug($"Loaded config achievement \"{achievement.Name}\"");
            AchievementManager.RegisterAchievement(achievement);
        }

        // Initialize achievements system
        AchievementManager.Initialize();

        // Initialize UI assets
        AchievementAssets.Load();

        Instantiate(AchievementAssets.UIAssets).AddComponent<HUDController>();

        // Create mod tabs and a list of achievements for each
        HUDController.Instance!.InitializeUI();

        // Hide UI on start
        HUDController.Instance.gameObject.SetActive(false);
    }
}