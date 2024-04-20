using System;
using LethalAchievements.Features;
using LethalModDataLib.Enums;
using UnityEngine;

namespace LethalAchievements.Interfaces;

/// <summary>
///     Interface for an achievement.
/// </summary>
public interface IAchievement
{
    /// <summary>
    ///     The name of the achievement.
    /// </summary>
    public string Name { get; internal set; }

    /// <summary>
    ///     The display text of the achievement.
    /// </summary>
    /// <remarks> Needs to be below ~100 characters. Used for tip popup type. </remarks>
    public string? DisplayText { get; internal set; }

    /// <summary>
    ///     The description of the achievement. Used instead of the DisplayText for the achievement menu (if not null).
    /// </summary>
    public string? Description { get; internal set; }

    /// <summary>
    ///     The progress text of the achievement. Can be null if the achievement is not progress-based.
    /// </summary>
    public string? ProgressText { get; internal set; }

    /// <summary>
    ///     The target progress of the achievement. Can be null if the achievement is not progress-based.
    /// </summary>
    public float? ProgressTarget { get; internal set; }

    /// <summary>
    ///     Whether the achievement is achieved.
    /// </summary>
    public bool IsAchieved { get; internal set; }

    /// <summary>
    ///     Whether the achievement is hidden in the achievement menu.
    /// </summary>
    public bool IsHidden { get; internal set; }

    /// <summary>
    ///     Save location of the achievement. Used to determine if the achievement is global or per save.
    /// </summary>
    /// <remarks>
    ///     <see cref="LethalModDataLib.Enums.SaveLocation.CurrentSave" /> is for per-save achievements, while
    ///     <see cref="SaveLocation.GeneralSave" /> is for global achievements.
    /// </remarks>
    public SaveLocation SaveLocation { get; internal set; }

    /// <summary>
    ///     The icon of the achievement.
    /// </summary>
    /// <remarks> Icons should be some multiple of 1x1 pixels. </remarks>
    public Sprite? Icon { get; internal set; }

    /// <summary>
    ///     Progress of the achievement. Can be null if the achievement is not progress-based.
    /// </summary>
    public float? GetProgress();

    /// <summary>
    ///     Initializes the achievement. Called by <see cref="AchievementRegistry.AddAchievement" />.
    ///     Should hook up any event listeners, etc...
    /// </summary>
    public void Initialize();

    /// <summary>
    ///     Uninitializes the achievement. Called by <see cref="AchievementRegistry.RemoveAchievement" /> and upon achievement
    ///     completion at <see cref="AchievementManager.OnAchieved" />.
    ///     Should unhook any event listeners, clean up, etc...
    /// </summary>
    public void Uninitialize();

    /// <summary>
    ///     Event to be invoked when the achievement is achieved. Informs the AchievementManager that the achievement was
    ///     completed.
    /// </summary>
    public event Action? AchievedEvent;
}