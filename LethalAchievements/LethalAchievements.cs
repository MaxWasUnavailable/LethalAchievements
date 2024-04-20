using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using LethalAchievements.Enums;
using LethalAchievements.Events.Patches;
using LethalAchievements.Features;
using LethalAchievements.Json;
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
    private bool _isPatched;
    internal new static ManualLogSource? Logger { get; private set; }
    private Harmony? Harmony { get; set; }

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

        // Patch using Harmony
        PatchAll();

        // Hook into post-game init event
        MiscEvents.PostInitializeGameEvent += OnGameLoaded;

        // Report plugin loaded
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }

    private static void OnGameLoaded()
    {
        Logger!.LogInfo("Game loaded, initializing achievements systems...");

        ArePluginsLoaded = true;

        // Load json achievements
        foreach (var achievement in JsonLoader.LoadAchievements(Paths.PluginPath))
        {
            if (achievement == null) continue;

            Logger.LogDebug($"Loaded config achievement \"{achievement.Name}\"");
            AchievementManager.RegisterAchievement(achievement);
        }

        // Initialize the achievements system
        AchievementManager.Initialize();

        if (AchievementAssets.Load())
        {
            // Initialize UI assets
            Instantiate(AchievementAssets.UIAssets)?.AddComponent<HUDController>();

            // Create mod tabs and a list of achievements for each
            HUDController.InitializeUI();
        }
        else
        {
            Logger.LogError("Failed to load UI assets! UI will not work! Are you missing the achievement_assets file?");
        }
    }

    private void PatchAll()
    {
        if (_isPatched)
        {
            Logger?.LogWarning("Already patched!");
            return;
        }

        Logger?.LogDebug("Patching...");

        Harmony ??= new Harmony(PluginInfo.PLUGIN_GUID);

        EnemyDamageSource.Patch(Harmony);
        Harmony.PatchAll();

        _isPatched = true;

        Logger?.LogDebug("Patched!");
    }
}