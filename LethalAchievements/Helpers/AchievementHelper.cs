using System.Linq;
using System.Reflection;
using BepInEx.Bootstrap;
using LethalAchievements.Interfaces;

namespace LethalAchievements.Helpers;

/// <summary>
///     Helper class for achievements.
/// </summary>
public static class AchievementHelper
{
    /// <summary>
    ///     Gets the GUID of the specified <see cref="IAchievement" />.
    /// </summary>
    /// <param name="achievement"> The <see cref="IAchievement" /> to get the GUID of. </param>
    /// <returns> The GUID of the specified <see cref="IAchievement" />. </returns>
    public static string GetAchievementGuid(this IAchievement achievement)
    {
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
    ///     Sends a chat message notifying player an achievement was achieved.
    /// </summary>
    /// <param name="achievement"> The <see cref="IAchievement" /> that was achieved. </param>
    public static void AchievementChatMessage(IAchievement achievement)
    {
        UIHelper.SendServerChatMessage(
            $"<b><color=#FFD700>{PlayerHelper.GetCurrentPlayerName()} unlocked achievement:</color></b><i><color=#FFFFFF>{achievement.Name}</color></i>");
    }

    /// <summary>
    ///     Use a status message to display an achievement.
    /// </summary>
    /// <param name="achievement"> The <see cref="IAchievement" /> to display. </param>
    public static void DisplayAchievementAsStatus(IAchievement achievement)
    {
        UIHelper.DisplayStatusMessage(
            $"<b><color=#FFD700>Achievement Unlocked!</color></b>\n\n<color=#FFFFFF>{achievement.Name}</color>");
    }

    /// <summary>
    ///     Use a tip to display an achievement.
    /// </summary>
    /// <param name="achievement"> The <see cref="IAchievement" /> to display. </param>
    public static void DisplayAchievementAsTip(IAchievement achievement)
    {
        UIHelper.DisplayTip(
            "<b><color=#FFD700>Achievement Unlocked!</color></b>",
            $"<color=#FFFFFF>{achievement.Name}</color>",
            true
        );
    }
}