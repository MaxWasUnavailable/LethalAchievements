using System;
using GameNetcodeStuff;
using LethalAchievements.Config.Predicates;
using LethalAchievements.Config.Serialization;
using LethalAchievements.Events;
using Newtonsoft.Json;
using static LethalAchievements.Config.ConditionHelper;

namespace LethalAchievements.Config.Triggers;

/// <summary>
///     Triggers when the local player dies.
/// </summary>
public class DieTrigger : ITrigger
{
    /// <summary>
    ///     Checks the velocity of the player's body.
    ///     In cases where the body is deleted (for example with Sandworms), this will have a value of 0.
    /// </summary>
    public FloatRange? Velocity;
    
    /// <summary>
    ///     Checks the cause of death. If you specify multiple causes, any of them can match.
    /// </summary>
    [JsonConverter(typeof(OneOrMultipleConverter))]
    public CauseOfDeath[]? Cause;

    /// <summary>
    ///     Checks the enemy that killed the player.
    ///     If this is specified, the cause of the damage must be an enemy (and match this predicate).
    /// </summary>
    public EnemyPredicate? Enemy;

    /// <inheritdoc />
    public event Action<Context>? OnTriggered;

    /// <inheritdoc />
    public void Initialize()
    {
        PlayerEvents.OnDied += OnDied;
    }

    /// <inheritdoc />
    public void Uninitialize()
    {
        PlayerEvents.OnDied -= OnDied;
    }

    private void OnDied(PlayerControllerB player, in PlayerDiedContext context)
    {
        if (!Matches(context.BodyVelocity.magnitude, Velocity)) return;
        if (!Contains(context.CauseOfDeath, Cause)) return;
        if (!Predicate(context.KillerEnemy, Enemy)) return;
        
        OnTriggered?.Invoke(Context.Default());
    }
}