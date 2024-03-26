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
    /// <remarks> Needs to be below 100 characters. Used for popup. </remarks> // TODO: Review character limit
    public abstract string? DisplayText { get; internal set; }
    
    /// <summary>
    ///     The description of the achievement.
    /// </summary>
    /// <remarks> Used in the achievement menu. </remarks>
    public abstract string? Description { get; internal set; }
    
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
    ///     Icon for the achievement.
    /// </summary>
    public abstract Sprite? Icon { get; internal set; }

    /// <summary>
    ///     Initializes the achievement. Called by <see cref="AchievementManager.AddAchievement"/>.
    ///     Should hook up any event listeners, etc...
    /// </summary>
    public abstract void Initialize();
    
    /// <summary>
    ///     Uninitializes the achievement. Called by <see cref="AchievementManager.RemoveAchievement"/> and upon achievement completion.
    ///     Should unhook any event listeners, clean up, etc...
    /// </summary>
    public abstract void Uninitialize();
    
    /// <summary>
    ///     Event to be invoked when the achievement is achieved. Informs the AchievementManager that the achievement was completed.
    /// </summary>
    public abstract event Action? AchievedEvent;
}