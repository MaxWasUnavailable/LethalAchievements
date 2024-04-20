using System;
using UnityEngine;

namespace LethalAchievements.Events;

/// <summary>
///     Provides callbacks for lifetime events.
/// </summary>
internal class LifetimeEvents : MonoBehaviour
{
    /// <summary>
    ///     Called every Update.
    /// </summary>
    public static event Action? OnUpdate;
    
    /// <summary>
    ///     Called every FixedUpdate.
    /// </summary>
    public static event Action? OnFixedUpdate;
    
    /// <summary>
    ///     Called every LateUpdate.
    /// </summary>
    public static event Action? OnLateUpdate;

    static LifetimeEvents()
    {
        var runner = new GameObject("LifetimeEventsRunner").AddComponent<LifetimeEvents>();
        DontDestroyOnLoad(runner.gameObject);
    }
    
    private void Update()
    {
        OnUpdate?.Invoke();
    }
        
    private void FixedUpdate()
    {
        OnFixedUpdate?.Invoke();
    }
        
    private void LateUpdate()
    {
        OnLateUpdate?.Invoke();
    }
}