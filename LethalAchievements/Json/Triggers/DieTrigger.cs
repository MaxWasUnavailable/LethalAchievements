using System;
using GameNetcodeStuff;
using LethalAchievements.Config;
using LethalAchievements.Events;
using LethalAchievements.Json.Predicates;
using LethalAchievements.Json.Serialization;
using Newtonsoft.Json;
using static LethalAchievements.Config.ConditionHelper;

namespace LethalAchievements.Json.Triggers;

/// <summary>
///     Triggers when the local player dies.
/// </summary>
public class DieTrigger : ITrigger
{
    /// <summary>
    ///     Checks the cause of death. If you specify multiple causes, any of them can match.
    /// </summary>
    [JsonConverter(typeof(OneOrMultipleConverter<CauseOfDeath>))]
    public CauseOfDeath[]? Cause;

    /// <summary>
    ///     Checks the enemy that killed the player.
    ///     If this is specified, the cause of the damage must be an enemy (and match this predicate).
    /// </summary>
    public EnemyPredicate? Enemy;

    /// <summary>
    ///     Checks the velocity of the player's body.
    /// </summary>
    /// <remarks>
    ///     Causes that delete the player body (such as Sandworms) will result in a velocity of 0.
    /// </remarks>
    public FloatRange? Velocity;

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