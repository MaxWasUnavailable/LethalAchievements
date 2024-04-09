using System;
using GameNetcodeStuff;
using LethalAchievements.Config.Predicates;
using LethalAchievements.Config.Serialization;
using LethalAchievements.Events;
using Newtonsoft.Json;
using UnityEngine;
using static LethalAchievements.Config.ConditionHelper;

namespace LethalAchievements.Config.Triggers;

public class DieTrigger : ITrigger
{
    public FloatRange? Velocity;
    
    [JsonConverter(typeof(OneOrMultipleConverter<CauseOfDeath>))]
    public CauseOfDeath[]? Cause;

    public EnemyPredicate? EnemyKiller;
    
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

    private void OnDied(PlayerControllerB player, in PlayerDiedContext context) {
        if (!Matches(context.BodyVelocity.magnitude, Velocity)) return;
        if (!Contains(context.CauseOfDeath, Cause)) return;
        if (!Predicate(context.KillerEnemy, EnemyKiller)) return;
        
        OnTriggered?.Invoke(Context.Default());
    }
}