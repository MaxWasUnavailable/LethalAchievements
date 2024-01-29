using System;

namespace LethalAchievements.Interfaces;

public interface IAchievement
{
    event Action? AchievedEvent;
    public string Name { get; internal set; }
    public string? Description { get; internal set; }
    public bool IsAchieved { get; internal set; }
    public bool IsHidden { get; internal set; }

    public void Initialize();
    public void Uninitialize();
}