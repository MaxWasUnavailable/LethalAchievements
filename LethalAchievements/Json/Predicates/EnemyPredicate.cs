using LethalAchievements.Config.Serialization;
using Newtonsoft.Json;
using static LethalAchievements.Config.ConditionHelper;

namespace LethalAchievements.Config.Predicates;

/// <summary>
///     Checks properties of an enemy.
/// </summary>
public class EnemyPredicate : IPredicate<EnemyAI>
{
    /// <summary>
    ///     Checks the health of the enemy. Can be a range of values.
    /// </summary>
    public IntRange? Health;
    
    /// <summary>
    ///     Checks whether the enemy spawns outside the facility (Forest giants, sandworms e.t.c).
    /// </summary>
    // TODO: check if masked are classified as inside or outside
    public bool? Outside;
    
    /// <summary>
    ///     Checks the name of the enemy.
    /// </summary>
    [JsonConverter(typeof(OneOrMultipleConverter))]
    public string[]? Name;

    /// <summary>
    ///     Checks the held item of the enemy.
    ///     Only applicable for loot bugs and nutcrackers.
    ///     If this is specified and the enemy is not holding an item, the predicate will fail.
    /// </summary>
    public ItemPredicate? HeldItem;
    
    /// <inheritdoc />
    public bool Check(EnemyAI enemy) {
        var type = enemy.enemyType;

        if (HeldItem != null) {
            var heldItem = enemy switch {
                NutcrackerEnemyAI nutcracker => nutcracker.gun,
                HoarderBugAI lootBug => lootBug.heldItem.itemGrabbableObject,
                _ => null
            };
            
            if (!Predicate(heldItem, HeldItem)) return false;
        }
        
        return All(
            Predicate(enemy.enemyHP, Health),
            Predicate(enemy.isOutside, Outside),
            Contains(type.name, Name)
        );
    }
}