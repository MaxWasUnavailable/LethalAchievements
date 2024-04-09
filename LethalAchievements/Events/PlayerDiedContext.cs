using UnityEngine;

namespace LethalAchievements.Events;

internal readonly struct PlayerDiedContext
{
    public readonly Vector3 BodyVelocity;
    public readonly CauseOfDeath CauseOfDeath;
    public readonly EnemyAI? KillerEnemy;

    public PlayerDiedContext(Vector3 bodyVelocity, CauseOfDeath causeOfDeath, EnemyAI? killerEnemy)
    {
        BodyVelocity = bodyVelocity;
        CauseOfDeath = causeOfDeath;
        KillerEnemy = killerEnemy;
    }
}