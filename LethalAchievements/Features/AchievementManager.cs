using System;
using System.Collections.Generic;
using System.Linq;
using LethalAchievements.Enums;
using LethalAchievements.Helpers;
using LethalAchievements.Interfaces;
using LethalModDataLib.Events;

namespace LethalAchievements.Features;

/// <summary>
///     Manager for achievements.
/// </summary>
public static class AchievementManager
{
    internal static AchievementRegistry AchievementRegistry { get; } = new();
    private static List<IAchievement> AchievementsToAdd { get; } = new();

    /// <summary>
    ///     Registers an achievement with the achievement manager.
    /// </summary>
    /// <param name="achievement"> The <see cref="IAchievement" /> to register. </param>
    /// <remarks> Actual registration is done after all plugins are loaded. </remarks>
    public static void RegisterAchievement(IAchievement achievement)
    {
        LethalAchievements.Logger?.LogDebug(
            $"Registering achievement \"{achievement.Name}\" from \"{achievement.GetType().Assembly.FullName}\"...");
        AchievementsToAdd.Add(achievement);
    }

    /// <summary>
    ///     Adds all registered achievements to the achievement registry.
    ///     Done after the ChainLoader has loaded all plugins, since otherwise GUID of plugins is not available.
    /// </summary>
    private static void AddRegisteredAchievements()
    {
        foreach (var achievement in AchievementsToAdd.Where(achievement =>
                     AchievementRegistry.AddAchievement(achievement)))
            achievement.AchievedEvent += () => OnAchieved(achievement);
        AchievementsToAdd.Clear();
    }

    /// <summary>
    ///     Initializes the achievement manager.
    /// </summary>
    internal static void Initialize()
    {
        AddRegisteredAchievements();
        SaveLoadEvents.PostLoadGameEvent += OnLoadGame;
    }

    /// <summary>
    ///     Initializes all achievements that aren't already achieved.
    /// </summary>
    private static void InitializeAchievements()
    {
        foreach (var achievement in AchievementRegistry.GetAchievements())
        {
            // We always load the IsAchieved state of the achievement since this might vary between saves
            achievement.LoadAchievedState();
            // If the achievement is not achieved, we initialize it
            if (!achievement.IsAchieved)
            {
                LethalAchievements.Logger?.LogDebug($"Initializing achievement \"{achievement.Name}\"...");
                achievement.Initialize();
            }
        }
    }

    /// <summary>
    ///     Uninitializes all achievements.
    /// </summary>
    private static void UninitializeAchievements()
    {
        foreach (var achievement in AchievementRegistry.GetAchievements()) achievement.Uninitialize();
    }

    /// <summary>
    ///     Called when an achievement is achieved. Used to handle achievement completion events.
    /// </summary>
    /// <param name="achievement"> The <see cref="IAchievement" /> that was achieved. </param>
    private static void OnAchieved(IAchievement achievement)
    {
        if (LethalAchievements.AchievementSoundEnabled!.Value)
            SoundHelper.PlayLevelUpSound();

        switch (LethalAchievements.AchievementPopupStyle!.Value)
        {
            case AchievementPopupStyle.StatusMessage:
                achievement.DisplayAsStatus();
                break;
            case AchievementPopupStyle.Tip:
                achievement.DisplayAsTip(true);
                break;
            case AchievementPopupStyle.GlobalNotification:
                achievement.DisplayAsGlobalNotification();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        achievement.SendChatMessage();

        achievement.IsAchieved = true;
        achievement.SaveAchievedState();
        achievement.Uninitialize();

        LethalAchievements.Logger?.LogDebug($"Achievement \"{achievement.Name}\" achieved!");
    }

    /// <summary>
    ///     Called when the game is loaded. Used to initialize achievements.
    /// </summary>
    private static void OnLoadGame(bool isChallenge, string saveFileName)
    {
        // TODO: May need to wait a couple of seconds so UI & the like are initialized
        InitializeAchievements();
    }
}