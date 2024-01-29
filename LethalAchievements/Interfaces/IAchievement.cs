using System;
using UnityEngine;

namespace LethalAchievements.Interfaces;

public interface IAchievement
{
    public string Name { get; internal set; }
    public string? Description { get; internal set; }
    public bool IsAchieved { get; internal set; }
    public bool IsHidden { get; internal set; }
    public Sprite? Icon { get; internal set; }
    event Action? AchievedEvent;

    public void Initialize();
    public void Uninitialize();
}