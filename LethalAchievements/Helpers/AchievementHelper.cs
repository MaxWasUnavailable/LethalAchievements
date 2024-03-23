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
    public static string GetAchievementGuid(IAchievement achievement)
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
}