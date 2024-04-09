namespace LethalAchievements.Events;

/// <summary>
///     Context for the <see cref="PlayerEvents.OnDamaged"/> event.
/// </summary>
internal struct PlayerDamagedContext
{
    /// <summary>
    ///    The amount of damage dealt to the player.
    /// </summary>
    public int Amount;

    /// <summary>
    ///     The cause of death if the player dies.
    /// </summary>
    public CauseOfDeath CauseOfDeath;

    /// <summary>
    ///    The enemy that damaged the player, if any.
    /// </summary>
    public EnemyAI? AttackerEnemy;

    public PlayerDamagedContext(int amount, CauseOfDeath causeOfDeath, EnemyAI? attackerEnemy)
    {
        Amount = amount;
        CauseOfDeath = causeOfDeath;
        AttackerEnemy = attackerEnemy;
    }
}