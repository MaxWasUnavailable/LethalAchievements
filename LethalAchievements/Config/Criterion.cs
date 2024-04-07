using System;
using Newtonsoft.Json;

namespace LethalAchievements.Config;

public class Criterion
{
    [JsonRequired]
    public ITrigger Trigger { get; }
    public ICondition[]? Conditions { get; }

    private Action<Criterion>? _callback;
    
    /// <summary>
    ///     Adds a callback to be invoked when the trigger is triggered.
    ///     Note: this does not check the conditions!
    /// </summary>
    public void Subscribe(Action<Criterion> callback)
    {
        if (_callback is null)
        {
            // do this only the first time Subscribe is called
            Trigger.OnTriggered += OnTriggered;
        }
        _callback += callback;
    }
    
    /// <summary>
    ///    Removes a callback from being invoked when the trigger is triggered.
    /// </summary>
    public void Unsubscribe(Action<Criterion> callback)
    {
        _callback -= callback;
        if (_callback is null)
        {
            // do this only the last time callback is removed
            Trigger.OnTriggered -= OnTriggered;
        }
    }

    private void OnTriggered()
    {
        _callback?.Invoke(this);
    }
}