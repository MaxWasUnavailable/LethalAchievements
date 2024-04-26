using UnityEngine;

namespace LethalAchievements.Events;

/// <summary>
///     Context for the <see cref="PlayerEvents.OnDied" /> event.
/// </summary>
internal struct PlayerDiedContext(Vector3 bodyVelocity, CauseOfDeath causeOfDeath, EnemyAI? killerEnemy)
{
    /// <summary>
    ///     The velocity applied to the player's body.
    /// </summary>
    public Vector3 BodyVelocity = bodyVelocity;

    /// <summary>
    ///     The cause of death.
    /// </summary>
    public CauseOfDeath CauseOfDeath = causeOfDeath;

    /// <summary>
    ///     The enemy that killed the player, if any.
    /// </summary>
    public EnemyAI? KillerEnemy = killerEnemy;
}