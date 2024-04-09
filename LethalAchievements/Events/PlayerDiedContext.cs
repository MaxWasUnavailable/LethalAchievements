using UnityEngine;

namespace LethalAchievements.Events;

/// <summary>
///     Context for the <see cref="PlayerEvents.OnDied"/> event.
/// </summary>
internal struct PlayerDiedContext
{
    /// <summary>
    ///     The velocity applied to the player's body.
    /// </summary>
    public Vector3 BodyVelocity;

    /// <summary>
    ///     The cause of death.
    /// </summary>
    public CauseOfDeath CauseOfDeath;

    /// <summary>
    ///     The enemy that killed the player, if any.
    /// </summary>
    public EnemyAI? KillerEnemy;

    public PlayerDiedContext(Vector3 bodyVelocity, CauseOfDeath causeOfDeath, EnemyAI? killerEnemy)
    {
        BodyVelocity = bodyVelocity;
        CauseOfDeath = causeOfDeath;
        KillerEnemy = killerEnemy;
    }
}