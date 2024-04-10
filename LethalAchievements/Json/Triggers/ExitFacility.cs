using System;
using GameNetcodeStuff;
using LethalAchievements.Events;
using static LethalAchievements.Config.ConditionHelper;

namespace LethalAchievements.Config.Triggers;

/// <summary>
///     Triggered when the local player exits the facility.
/// </summary>
public class ExitFacility : ITrigger
{
    /// <summary>
    ///     Checks whether the player entered through a fire exit.
    /// </summary>
    public bool? FireExit;
    
    /// <inheritdoc />
    public event Action<Context>? OnTriggered;

    /// <inheritdoc />
    public void Initialize() 
    {
        PlayerEvents.OnExitedFacility += OnEnteredFacility;
    }

    /// <inheritdoc />
    public void Uninitialize()
    {
        PlayerEvents.OnExitedFacility -= OnEnteredFacility;
    }
    
    void OnEnteredFacility(PlayerControllerB player, in EntranceType context)
    {
        if (!player.IsOwner) return;
        if (!Matches(context == EntranceType.Fire, FireExit)) return;
        
        OnTriggered?.Invoke(Context.Default());
    }
}