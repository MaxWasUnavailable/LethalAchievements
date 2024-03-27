using System;
using System.Collections.Generic;
using System.Linq;
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
    ///     Registers an achievement with the achievement manager.
    /// </summary>
    /// <param name="achievement"> The <see cref="IAchievement" /> to register. </param>
    /// <returns> <see langword="true" /> if the achievement was registered successfully, <see langword="false" /> otherwise. </returns>
    public static bool RegisterAchievement(IAchievement achievement)
    {
        LethalAchievements.Logger?.LogDebug(
            $"Registering achievement \"{achievement.Name}\" from \"{achievement.GetType().Assembly.FullName}\"...");
        return AddAchievement(achievement);
    }

    /// <summary>
    ///     Get an achievement by its type.
    /// </summary>
    /// <typeparam name="T"> The type of the achievement to get. </typeparam>
    /// <returns> The achievement of type <typeparamref name="T" />. </returns>
    public static T? GetAchievement<T>() where T : IAchievement, new()
    {
        var achievement = Achievements.Values.FirstOrDefault(achievement => achievement.GetType() == typeof(T));
        return achievement != null ? (T)achievement : default;
    }

    /// <summary>
    ///     Get an achievement by its type and name.
    /// </summary>
    /// <param name="name"> The name of the achievement to get. </param>
    /// <typeparam name="T"> The type of the achievement to get. </typeparam>
    /// <returns> The achievement of type <typeparamref name="T" /> with the name <paramref name="name" />. </returns>
    /// <remarks> Might be redundant, but made available in case it might be useful. </remarks>
    public static T? GetAchievement<T>(string name) where T : IAchievement, new()
    {
        var achievement = Achievements.Values.FirstOrDefault(achievement =>
            achievement.GetType() == typeof(T) &&
            string.Equals(achievement.Name, name, StringComparison.CurrentCultureIgnoreCase));
        return achievement != null ? (T)achievement : default;
    }

    /// <summary>
    ///     Adds an achievement to the achievement dictionary.
    /// </summary>
    /// <param name="achievement"> The <see cref="IAchievement" /> to add. </param>
    /// <returns> <see langword="true" /> if the achievement was added successfully, <see langword="false" /> otherwise. </returns>
    internal static bool AddAchievement(IAchievement achievement)
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
    internal static bool RemoveAchievement(IAchievement achievement)
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
        var message =
            $"<b><color=#FFD700>Achievement Unlocked!</color></b>\n<i><color=#FFFFFF>{achievement.Name}</color></i>";
        HUDManager.Instance.AddTextToChatOnServer(message);
    }

    // TODO: UI for achievements
    // TODO: Achievement save/load
}