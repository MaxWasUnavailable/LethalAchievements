using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using LethalAchievements.Achievements;
using LethalAchievements.Features;

namespace LethalAchievements;

/// <summary>
///     Main plugin class for LethalAchievements.
/// </summary>
[BepInDependency(LethalEventsLib.PluginInfo.PLUGIN_GUID)]
[BepInDependency(LethalModDataLib.PluginInfo.PLUGIN_GUID)]
[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class LethalAchievements : BaseUnityPlugin
{
    private bool _isPatched;
    private Harmony? Harmony { get; set; }
    internal new static ManualLogSource? Logger { get; private set; }

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

        // Patch using Harmony
        PatchAll();
        
        // Register example achievements
        // TODO: remove for release
        AchievementManager.RegisterAchievement(new JumpAchievement());
        AchievementManager.RegisterAchievement(new Jump10Achievement());
        AchievementManager.RegisterAchievement(new GeneralJumpAchievement());
        
        // Hook into post game init event -- We reuse the one from LethalModDataLib since we're using that library anyway
        LethalModDataLib.Events.MiscEvents.PostInitializeGameEvent += OnGameLoaded;

        // Plugin startup logic
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
    }
    
    private static void OnGameLoaded()
    {
        // Add registered achievements
        AchievementManager.AddRegisteredAchievements();
    }

    /// <summary>
    ///     Patch all methods with Harmony.
    /// </summary>
    public void PatchAll()
    {
        if (_isPatched)
        {
            Logger?.LogWarning("Already patched!");
            return;
        }

        Logger?.LogDebug("Patching...");

        Harmony ??= new Harmony(PluginInfo.PLUGIN_GUID);

        Harmony.PatchAll();
        _isPatched = true;

        Logger?.LogDebug("Patched!");
    }

    /// <summary>
    ///     Unpatch all methods with Harmony.
    /// </summary>
    public void UnpatchAll()
    {
        if (!_isPatched)
        {
            Logger?.LogWarning("Already unpatched!");
            return;
        }

        Logger?.LogDebug("Unpatching...");

        Harmony!.UnpatchSelf();
        _isPatched = false;

        Logger?.LogDebug("Unpatched!");
    }
}