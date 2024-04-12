using System;
using GameNetcodeStuff;
using LethalAchievements.Config.Predicates;
using LethalAchievements.Events;

namespace LethalAchievements.Config.Triggers;

/// <summary>
///     Triggered when the local player kills an enemy.
/// </summary>
/// <example>
/// <code>
/// {
///     "display_text": "Kill a Nutcracker with a Shotgun",
///     "criteria": {
///         "trigger": "kill_enemy",
///         "enemy": {
///             "name": "Nutcracker"
///         },
///         "conditions": {
///             "player": {
///                 "held_item": {
///                     "name": "Shotgun"
///                 }
///             }
///         }
///     }
/// }
/// </code>
/// </example>
public class KillEnemyTrigger : ITrigger
{
    /// <summary>
    ///     Checks the enemy that was killed.
    ///     Checked right before the enemy takes fatal damage from the player.
    /// </summary>
    public EnemyPredicate? Enemy;
    
    /// <inheritdoc />
    public event Action<Context>? OnTriggered;

    /// <inheritdoc />
    public void Initialize()
    {
        PlayerEvents.OnKilledEnemy += OnKilledEnemy;
    }

    /// <inheritdoc />
    public void Uninitialize()
    {
        PlayerEvents.OnKilledEnemy += OnKilledEnemy;
    }
    
    private void OnKilledEnemy(PlayerControllerB player, in EnemyAI enemy)
    {
        if (!player.IsOwner) return;
        if (!ConditionHelper.Predicate(enemy, Enemy)) return;
        
        OnTriggered?.Invoke(Context.Default());
    }
}