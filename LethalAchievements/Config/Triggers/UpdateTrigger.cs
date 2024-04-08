using System;
using LethalAchievements.Events;

namespace LethalAchievements.Config.Triggers;

/// <summary>
///     An achievement trigger which is triggered every frame.
/// </summary>
public class UpdateTrigger : ITrigger
{
    /// <inheritdoc />
    public event Action<Context>? OnTriggered;

    /// <inheritdoc />
    public void Initialize()
    {
        LifetimeEvents.OnUpdate += OnUpdate;
    }

    /// <inheritdoc />
    public void Uninitialize()
    {
        LifetimeEvents.OnUpdate -= OnUpdate;
    }

    private void OnUpdate()
    {
        OnTriggered?.Invoke(Context.Default());
    }
}