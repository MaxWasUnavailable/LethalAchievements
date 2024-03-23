using System;
using LethalAchievements.Interfaces;
using LethalModDataLib.Enums;
using UnityEngine;

namespace LethalAchievements.Base;

public abstract class BaseAchievement : IAchievement
{
    // Kept abstract for modders to implement
    public abstract string Name { get; set; }
    public abstract string? ShortDescription { get; set; }
    public abstract string? LongDescription { get; set; }
    public abstract Sprite? Icon { get; set; }
    public abstract void Initialize();
    public abstract void Uninitialize();
    
    // IsHidden will generally not be changed by modders, set to false for convenience
    public bool IsHidden { get; set; } = false;
    
    // SaveLocation will generally not be changed by modders, set to CurrentSave for convenience
    public SaveLocation SaveLocation { get; set; } = SaveLocation.CurrentSave;
    
    public event Action? AchievedEvent;
    
    // Added virtual for modders to override
    protected virtual void Complete()  // TODO: Rename to Achieve?
    {
        AchievedEvent?.Invoke();
    }
}