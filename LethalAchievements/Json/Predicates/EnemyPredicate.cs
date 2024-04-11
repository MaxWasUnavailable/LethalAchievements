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
    
    /// <inheritdoc />
    public bool Check(EnemyAI enemy) {
        var type = enemy.enemyType;
        
        return All(
            Matches(enemy.enemyHP, Health),
            Matches(enemy.isOutside, Outside),
            Contains(type.name, Name)
        );
    }
}