using System;
using LethalAchievements.Events;

namespace LethalAchievements.Config.Triggers;

/// <summary>
///     Triggers when a new profit quota is set.
/// </summary>
public class NewQuotaTrigger : ITrigger 
{
    /// <inheritdoc />
    public event Action<Context>? OnTriggered;
    
    /// <inheritdoc />
    public void Initialize() {
        QuotaEvents.OnNewQuota += OnNewQuota;
    }
    
    /// <inheritdoc />
    public void Uninitialize() {
        QuotaEvents.OnNewQuota -= OnNewQuota;
    }
    
    private void OnNewQuota() {
        OnTriggered?.Invoke(Context.Default());
    }
}