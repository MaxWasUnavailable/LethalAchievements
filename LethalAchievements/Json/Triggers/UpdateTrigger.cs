using System;
using LethalAchievements.Config;
using LethalAchievements.Events;
using Newtonsoft.Json;

namespace LethalAchievements.Json.Triggers;

/// <summary>
///     Triggered every frame.
/// </summary>
public class UpdateTrigger : ITrigger
{
    private int _counter;

    /// <summary>
    ///     The interval at which the trigger is triggered, defaults to 1 (every frame).
    ///     Values lower than 1 are clamped to 1.
    /// </summary>
    [JsonProperty("every")] public int Interval = 1;

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
        _counter++;
        if (_counter < Interval) return;
        _counter = 0;

        OnTriggered?.Invoke(Context.Default());
    }
}