using System;
using LethalAchievements.Features;
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
    ///     The short description of the achievement.
    /// </summary>
    /// <remarks> Needs to be below 100 characters. Used for popup. </remarks> // TODO: Review character limit
    public abstract string? ShortDescription { get; internal set; }
    
    /// <summary>
    ///     The long description of the achievement.
    /// </summary>
    /// <remarks> Used in the achievement menu. </remarks>
    public abstract string? LongDescription { get; internal set; }
    
    /// <summary>
    ///     Whether the achievement is hidden in the achievement menu.
    /// </summary>
    public abstract bool IsHidden { get; internal set; }
    
    /// <summary>
    ///     Icon for the achievement.
    /// </summary>
    public abstract Sprite? Icon { get; internal set; }

    /// <summary>
    ///     Initializes the achievement. Called by <see cref="AchievementManager.AddAchievement"/>.
    /// </summary>
    public abstract void Initialize();
    
    /// <summary>
    ///     Uninitializes the achievement. Called by <see cref="AchievementManager.RemoveAchievement"/>.
    /// </summary>
    public abstract void Uninitialize();
    
    /// <summary>
    ///     Event to be invoked when the achievement is achieved.
    /// </summary>
    event Action? AchievedEvent;
}