using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using LethalAchievements.Helpers;
using LethalAchievements.Interfaces;

namespace LethalAchievements.Features;

/// <summary>
///     Manager for achievements.
/// </summary>
public static class AchievementManager
{
    /// <summary>
    ///     Dictionary of all achievements. Key being the GUID of the achievement.
    /// </summary>
    private static Dictionary<string, IAchievement> Achievements { get; } = new();

    /// <summary>
    ///     Adds an achievement to the achievement dictionary.
    /// </summary>
    /// <param name="achievement"> The <see cref="IAchievement" /> to add. </param>
    /// <returns> <see langword="true" /> if the achievement was added successfully, <see langword="false" /> otherwise. </returns>
    public static bool AddAchievement(IAchievement achievement)
    {
        var achievementGuid = AchievementHelper.GetAchievementGuid(achievement);
        
        if (Achievements.ContainsKey(achievementGuid))
        {
            LethalAchievements.Logger?.LogWarning($"Achievement with guid \"{achievementGuid}\" already exists!");
            return false;
        }

        LethalAchievements.Logger?.LogDebug($"Adding achievement \"{achievementGuid}\"...");

        achievement.Initialize();
        Achievements.Add(achievementGuid, achievement);
        achievement.AchievedEvent += () => OnAchieved(achievement);

        return true;
    }

    /// <summary>
    ///     Removes an achievement from the achievement dictionary.
    /// </summary>
    /// <param name="achievement"> The <see cref="IAchievement" /> to remove. </param>
    /// <returns> <see langword="true" /> if the achievement was removed successfully, <see langword="false" /> otherwise. </returns>
    public static bool RemoveAchievement(IAchievement achievement)
    {
        var achievementGuid = AchievementHelper.GetAchievementGuid(achievement);
        if (!Achievements.ContainsKey(achievementGuid))
        {
            LethalAchievements.Logger?.LogWarning($"Achievement with guid \"{achievementGuid}\" does not exist!");
            return false;
        }

        LethalAchievements.Logger?.LogDebug($"Removing achievement \"{achievementGuid}\"...");

        achievement.Uninitialize();
        Achievements.Remove(achievementGuid);

        return true;
    }

    /// <summary>
    ///     Finds all achievements in the current domain and adds them to the list of achievements.
    /// </summary>
    internal static void FindAllAchievements()
    {
        LethalAchievements.Logger?.LogDebug("Finding all achievements...");

        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(IAchievement).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);

        var success = 0;
        var failed = 0;

        foreach (var type in types)
            try
            {
                if (AddAchievement((IAchievement)Activator.CreateInstance(type)!))
                    success++;
                else
                    failed++;
            }
            catch (MissingMethodException e)
            {
                LethalAchievements.Logger?.LogError(
                    $"Failed to load achievement \"{type.Name}\" due to missing constructor! {e}");
                failed++;
            }
            catch (TargetException e)
            {
                LethalAchievements.Logger?.LogError(
                    $"Failed to load achievement \"{type.Name}\" due to target exception! {e}");
                failed++;
            }
            catch (InvalidCastException e)
            {
                LethalAchievements.Logger?.LogError(
                    $"Failed to load achievement \"{type.Name}\" due to invalid cast! {e}");
                failed++;
            }

        LethalAchievements.Logger?.LogInfo($"Found {success} achievements, {failed} failed to load!");
    }

    /// <summary>
    ///     Called when an achievement is achieved. Used to handle achievement events.
    /// </summary>
    /// <param name="achievement"> The <see cref="IAchievement" /> that was achieved. </param>
    private static void OnAchieved(IAchievement achievement)
    {
        AchievementPopup(achievement);
        AchievementChatMessage(achievement);

        achievement.Uninitialize();

        LethalAchievements.Logger?.LogDebug($"Achievement \"{achievement.Name}\" achieved!");
    }

    /// <summary>
    ///     Displays a popup when an achievement is achieved.
    /// </summary>
    /// <param name="achievement"> The <see cref="IAchievement" /> that was achieved. </param>
    private static void AchievementPopup(IAchievement achievement)
    {
        // TODO: This is a placeholder system, replace with something better (e.g. rectangular popup in bottom-centre)
        HUDManager.Instance.UIAudio.PlayOneShot(HUDManager.Instance.levelIncreaseSFX);
        HUDManager.Instance.playerLevelBoxAnimator.SetTrigger("Shake");
        HUDManager.Instance.DisplayStatusEffect(
            $"<b><color=#FFD700>Achievement Unlocked!</color></b>\n\n<color=#FFFFFF>{achievement.Name}</color>");
    }

    /// <summary>
    ///     Sends a chat message when an achievement is achieved. Only visible to the player who achieved the achievement.
    /// </summary>
    /// <param name="achievement"> The <see cref="IAchievement" /> that was achieved. </param>
    private static void AchievementChatMessage(IAchievement achievement)
    {
        HUDManager.Instance.AddChatMessage($"<b><color=#FFD700>Achievement Unlocked!</color></b>\n<i><color=#FFFFFF>{achievement.Name}</color></i>");
    }

    // TODO: UI for achievements
    // TODO: Achievement save/load
}