using System;
using GameNetcodeStuff;
using LethalAchievements.Config.Predicates;
using LethalAchievements.Events;

namespace LethalAchievements.Config.Triggers;

/// <summary>
///     Triggered when the local player damages an enemy.
/// </summary>
public class DamageEnemyTrigger : ITrigger
{
    /// <summary>
    ///     Checks the enemy that was damaged.
    ///     Checked right before the enemy takes damage from the player.
    /// </summary>
    public EnemyPredicate? Enemy;
    
    /// <inheritdoc />
    public event Action<Context>? OnTriggered;

    /// <inheritdoc />
    public void Initialize()
    {
        PlayerEvents.OnDamagedEnemy += OnDamagedEnemy;
    }

    /// <inheritdoc />
    public void Uninitialize()
    {
        PlayerEvents.OnDamagedEnemy += OnDamagedEnemy;
    }
    
    private void OnDamagedEnemy(PlayerControllerB player, in EnemyAI enemy)
    {
        if (!player.IsOwner)
            return;
        
        OnTriggered?.Invoke(Context.Default());
    }
}