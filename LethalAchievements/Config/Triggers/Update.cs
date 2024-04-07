using System;
using GameNetcodeStuff;
using LethalAchievements.Config.Events;

namespace LethalAchievements.Config.Triggers;

/// <summary>
///     An achievement trigger which is triggered every frame.
/// </summary>
public class Update : ITrigger
{
    /// <inheritdoc />
    public event Action? OnTriggered;

    /// <inheritdoc />
    public void Initialize()
    {
        LifetimeEvents.OnUpdate += OnTriggered;
    }

    /// <inheritdoc />
    public void Uninitialize()
    {
        LifetimeEvents.OnUpdate -= OnTriggered;
    }
}