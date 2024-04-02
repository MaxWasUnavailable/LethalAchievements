using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx.Bootstrap;
using LethalAchievements.Interfaces;
using LethalModDataLib.Features;

namespace LethalAchievements.Helpers;

/// <summary>
///     Helper class for achievements.
/// </summary>
public static class AchievementHelper
{
    private static Dictionary<IAchievement, BepInEx.PluginInfo> AchievementsPluginCache { get; } = new();
    
    /// <summary>
    ///     Gets the GUID of the specified <see cref="IAchievement" />.
    /// </summary>
    /// <param name="achievement"> The <see cref="IAchievement" /> to get the GUID of. </param>
    /// <returns> The GUID of the specified <see cref="IAchievement" />. </returns>
    public static string GetAchievementGuid(this IAchievement achievement)
    {
        if (!LethalAchievements.ArePluginsLoaded)
            throw new InvalidOperationException("BePinEx has not finished loading plugins yet.");
        
        return $"{GetPluginGuid(achievement.GetType().Assembly)}.{achievement.Name}";
    }

    /// <summary>
    ///     Gets the GUID of the specified plugin assembly.
    /// </summary>
    /// <param name="pluginAssembly"> The plugin assembly to get the GUID of. </param>
    /// <returns> The GUID of the specified plugin assembly. </returns>
    private static string GetPluginGuid(Assembly pluginAssembly)
    {
        return Chainloader.PluginInfos
            .First(pluginInfo => pluginInfo.Value.Instance.GetType().Assembly == pluginAssembly).Value.Metadata.GUID;
    }
    
    /// <summary>
    ///     Gets the <see cref="PluginInfo" /> of the specified <see cref="IAchievement" />.
    /// </summary>
    /// <param name="achievement"> The <see cref="IAchievement" /> to get the <see cref="PluginInfo" /> of. </param>
    /// <returns> The <see cref="PluginInfo" /> of the specified <see cref="IAchievement" />. </returns>
    public static BepInEx.PluginInfo GetPluginInfo(this IAchievement achievement)
    {
        if (!LethalAchievements.ArePluginsLoaded)
            throw new InvalidOperationException("BePinEx has not finished loading plugins yet.");
        
        if (AchievementsPluginCache.TryGetValue(achievement, out var info))
            return info;

        AchievementsPluginCache.Add(achievement, Chainloader.PluginInfos
            .First(pluginInfo => pluginInfo.Value.Instance.GetType().Assembly == achievement.GetType().Assembly).Value);
        
        return AchievementsPluginCache[achievement];
    }

    /// <summary>
    ///     Load the IsAchieved state of a given achievement.
    /// </summary>
    /// <param name="achievement"> The <see cref="IAchievement" /> to load the IsAchieved state for. </param>
    internal static void LoadAchievedState(this IAchievement achievement)
    {
        achievement.IsAchieved =
            SaveLoadHandler.LoadData<bool>(achievement.GetAchievementGuid(), achievement.SaveLocation);
    }

    /// <summary>
    ///     Save the IsAchieved state of a given achievement.
    /// </summary>
    /// <param name="achievement"> The <see cref="IAchievement" /> to save the IsAchieved state for. </param>
    internal static void SaveAchievedState(this IAchievement achievement)
    {
        SaveLoadHandler.SaveData(achievement.IsAchieved, achievement.GetAchievementGuid(), achievement.SaveLocation);
    }

    /// <summary>
    ///     Sends a chat message notifying player an achievement was achieved.
    /// </summary>
    /// <param name="achievement"> The <see cref="IAchievement" /> that was achieved. </param>
    public static void SendChatMessage(this IAchievement achievement)
    {
        UIHelper.SendServerChatMessage(
            $"<b><color=#FFD700>{PlayerHelper.GetCurrentPlayerName()} unlocked achievement:</color></b><i><color=#FFFFFF>{achievement.Name}</color></i>");
    }

    /// <summary>
    ///     Use a status message to display an achievement.
    /// </summary>
    /// <param name="achievement"> The <see cref="IAchievement" /> to display. </param>
    public static void DisplayAsStatus(this IAchievement achievement)
    {
        UIHelper.DisplayStatusMessage(
            $"<b><color=#FFD700>Achievement Unlocked!</color></b>\n\n<color=#FFFFFF>{achievement.Name}</color>");
    }

    /// <summary>
    ///     Use a tip to display an achievement.
    /// </summary>
    /// <param name="achievement"> The <see cref="IAchievement" /> to display. </param>
    /// <param name="warningStyle"> Whether to use a warning style. </param>
    public static void DisplayAsTip(this IAchievement achievement, bool warningStyle = false)
    {
        UIHelper.DisplayTip(
            "<b><color=#FFD700>Achievement Unlocked!</color></b>",
            $"<color=#FFFFFF>{achievement.Name}</color>\n<color=#DDDDDD><i>{achievement.DisplayText}</i></color>",
            warningStyle
        );
    }

    /// <summary>
    ///     Use a global notification to display an achievement.
    /// </summary>
    /// <param name="achievement"> The <see cref="IAchievement" /> to display. </param>
    public static void DisplayAsGlobalNotification(this IAchievement achievement)
    {
        UIHelper.DisplayGlobalNotification(
            $"<b><color=#FFD700>Achievement Unlocked!</color></b>\n<color=#FFFFFF>{achievement.Name}</color>");
    }
}