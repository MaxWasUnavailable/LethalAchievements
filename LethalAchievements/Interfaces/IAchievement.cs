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
    public abstract string Name { get; internal set; }
    
    /// <summary>
    ///     The display text of the achievement.
    /// </summary>
    /// <remarks> Needs to be below ~100 characters. Used for tip popup type. </remarks>
    public abstract string? DisplayText { get; internal set; }
    
    /// <summary>
    ///     The description of the achievement. Used instead of the DisplayText for the achievement menu (if not null).
    /// </summary>
    public abstract string? Description { get; internal set; }
    
    /// <summary>
    ///     The progress text of the achievement. Can be null if the achievement is not progress-based.
    /// </summary>
    public abstract string? ProgressText { get; internal set; }
    
    /// <summary>
    ///     The target progress of the achievement. Can be null if the achievement is not progress-based.
    /// </summary>
    public abstract float? ProgressTarget { get; internal set; }

    /// <summary>
    ///     Progress of the achievement. Can be null if the achievement is not progress-based.
    /// </summary>
    public abstract float? GetProgress();
    
    /// <summary>
    ///     Whether the achievement is achieved.
    /// </summary>
    public abstract bool IsAchieved { get; internal set; }
    
    /// <summary>
    ///     Whether the achievement is hidden in the achievement menu.
    /// </summary>
    public abstract bool IsHidden { get; internal set; }
    
    /// <summary>
    ///     Save location of the achievement. Used to determine if the achievement is global or per save.
    /// </summary>
    /// <remarks> <see cref="SaveLocation.CurrentSave"/> is for per-save achievements, while <see cref="SaveLocation.GeneralSave"/> is for global achievements. </remarks>
    public abstract SaveLocation SaveLocation { get; internal set; }
    
    /// <summary>
    ///     The icon of the achievement.
    /// </summary>
    /// <remarks> Icons should be some multiple of 1x1 pixels. </remarks>
    public abstract Sprite? Icon { get; internal set; }

    /// <summary>
    ///     Initializes the achievement. Called by <see cref="AchievementRegistry.AddAchievement"/>.
    ///     Should hook up any event listeners, etc...
    /// </summary>
    public abstract void Initialize();
    
    /// <summary>
    ///     Uninitializes the achievement. Called by <see cref="AchievementRegistry.RemoveAchievement"/> and upon achievement completion at <see cref="AchievementManager.OnAchieved"/>.
    ///     Should unhook any event listeners, clean up, etc...
    /// </summary>
    public abstract void Uninitialize();
    
    /// <summary>
    ///     Event to be invoked when the achievement is achieved. Informs the AchievementManager that the achievement was completed.
    /// </summary>
    public abstract event Action? AchievedEvent;
}