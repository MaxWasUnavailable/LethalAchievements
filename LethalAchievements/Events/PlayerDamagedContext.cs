namespace LethalAchievements.Events;

internal readonly struct PlayerDamagedContext
{
    public readonly int Amount;
    public readonly CauseOfDeath CauseOfDeath;
    public readonly EnemyAI? AttackerEnemy;

    public PlayerDamagedContext(int amount, CauseOfDeath causeOfDeath, EnemyAI? attackerEnemy)
    {
        Amount = amount;
        CauseOfDeath = causeOfDeath;
        AttackerEnemy = attackerEnemy;
    }
}