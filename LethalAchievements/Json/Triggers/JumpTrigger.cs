using System;
using GameNetcodeStuff;
using LethalAchievements.Config;
using LethalAchievements.Events;

namespace LethalAchievements.Json.Triggers;

/// <summary>
///     Triggered when the local player jumps.
/// </summary>
public class JumpTrigger : ITrigger
{
    /// <inheritdoc />
    public event Action<Context>? OnTriggered;

    /// <inheritdoc />
    public void Initialize()
    {
        PlayerEvents.OnJumped += OnJumped;
    }

    /// <inheritdoc />
    public void Uninitialize()
    {
        PlayerEvents.OnJumped -= OnJumped;
    }

    private void OnJumped(PlayerControllerB player)
    {
        if (!player.IsOwner) return;
        OnTriggered?.Invoke(Context.Default());
    }
}