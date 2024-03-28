using System;
using LethalAchievements.Interfaces;
using LethalModDataLib.Enums;
using UnityEngine;

namespace LethalAchievements.Base;

/// <summary>
///     Abstract base class for an achievement. Provides some default implementations and a "Complete" method.
/// </summary>
public abstract class BaseAchievement : IAchievement
{
    // Kept abstract for modders to implement

    /// <inheritdoc />
    public abstract string Name { get; set; }

    /// <inheritdoc />
    public abstract string? DisplayText { get; set; }

    /// <inheritdoc />
    public abstract string? Description { get; set; }

    /// <inheritdoc />
    public abstract Sprite? Icon { get; set; }

    /// <inheritdoc />
    public abstract void Initialize();

    /// <inheritdoc />
    public abstract void Uninitialize();

    // IsHidden will generally not be changed by modders, set to false for convenience
    /// <inheritdoc />
    public virtual bool IsHidden { get; set; } = false;

    // SaveLocation will generally not be changed by modders, set to CurrentSave for convenience
    /// <inheritdoc />
    public virtual SaveLocation SaveLocation { get; set; } = SaveLocation.CurrentSave;
    
    // IsAchieved should be false by default
    /// <inheritdoc />
    public virtual bool IsAchieved { get; set; } = false;

    /// <inheritdoc />
    public virtual event Action? AchievedEvent;

    // Added virtual for modders to override
    /// <summary>
    ///     Completes the achievement.
    /// </summary>
    protected virtual void Complete()
    {
        AchievedEvent?.Invoke();
    }
}