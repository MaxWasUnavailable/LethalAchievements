using System;
using GameNetcodeStuff;
using LethalAchievements.Config.Predicates;
using LethalAchievements.Config.Serialization;
using LethalAchievements.Events;
using Newtonsoft.Json;
using static LethalAchievements.Config.ConditionHelper;

namespace LethalAchievements.Config.Triggers;

/// <summary>
///     Triggers when the local player takes damage.
///     This is not triggered by insta-death effects, such as Sandworms or falling into a pit.
/// </summary>
public class TakeDamageTrigger : ITrigger
{
    /// <summary>
    ///     Checks the amount of damage taken. Max player health is 100.
    /// </summary>
    public IntRange? Amount;
    
    /// <summary>
    ///     Checks the cause of the damage. If you specify multiple causes, any of them can match.
    /// </summary>
    [JsonConverter(typeof(OneOrMultipleConverter<CauseOfDeath>))]
    public CauseOfDeath[]? Cause;

    /// <summary>
    ///     Checks the enemy that caused the damage.
    ///     If this is specified, the cause of the damage must be an enemy (and match this predicate).
    /// </summary>
    public EnemyPredicate? Enemy;

    /// <inheritdoc />
    public event Action<Context>? OnTriggered;

    /// <inheritdoc />
    public void Initialize()
    {
        PlayerEvents.OnDamaged += OnDied;
    }

    /// <inheritdoc />
    public void Uninitialize()
    {
        PlayerEvents.OnDamaged -= OnDied;
    }

    private void OnDied(PlayerControllerB player, in PlayerDamagedContext context)
    {
        if (!Matches(context.Amount, Amount)) return;
        if (!Contains(context.CauseOfDeath, Cause)) return;
        if (!Predicate(context.AttackerEnemy, Enemy)) return;
        
        OnTriggered?.Invoke(Context.Default());
    }
}