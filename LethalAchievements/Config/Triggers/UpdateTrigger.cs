using System;
using LethalAchievements.Events;
using Newtonsoft.Json;

namespace LethalAchievements.Config.Triggers;

/// <summary>
///     An achievement trigger which is triggered every frame.
/// </summary>
public class UpdateTrigger : ITrigger
{
    [JsonProperty("every")]
    public int Interval = 1;
    
    private int _counter;
    
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