using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using LethalAchievements.Features;

namespace LethalAchievements;

[BepInDependency("LethalEventsLib")]
[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class LethalAchievements : BaseUnityPlugin
{
    private bool _isPatched;
    private Harmony? Harmony { get; set; }
    internal new static ManualLogSource? Logger { get; private set; }
    public static LethalAchievements? Instance { get; private set; }

    // Config entries
    public ConfigEntry<bool>? PlaceHolder { get; private set; }

    private void Awake()
    {
        // Set instance
        Instance = this;
        
        // Set do not destroy on load
        DontDestroyOnLoad(this);

        // Init logger
        Logger = base.Logger;

        // Init config entries
        PlaceHolder = Config.Bind("General", "PlaceHolder", true, "PlaceHolder");

        // Patch using Harmony
        PatchAll();

        // Plugin startup logic
        Logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        
        // Load achievements
        // TODO: This needs to be done after other plugins have loaded --> Event in LethalEventsLib when main menu is loaded?
        AchievementManager.FindAllAchievements();
    }

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