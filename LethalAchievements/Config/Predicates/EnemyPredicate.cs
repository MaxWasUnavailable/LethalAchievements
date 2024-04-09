﻿using LethalAchievements.Config.Serialization;
using Newtonsoft.Json;
using static LethalAchievements.Config.ConditionHelper;

namespace LethalAchievements.Config.Predicates;

/// <summary>
///     Checks properties of an enemy.
/// </summary>
public class EnemyPredicate : IPredicate<EnemyAI>
{
    /// <summary>
    ///     Checks whether the enemy spawns outside the facility.
    /// </summary>
    // TODO: check if masked are classified as inside or outside
    public bool? Outside;
    
    /// <summary>
    ///     Checks the name of the enemy. If you specify multiple, any of them will match.
    /// </summary>
    [JsonConverter(typeof(OneOrMultipleConverter<string>))]
    public string[]? Name;
    
    /// <inheritdoc />
    public bool Check(EnemyAI enemy) {
        var type = enemy.enemyType;
        
        return All(
            Matches(enemy.isOutside, Outside),
            Contains(type.name, Name)
        );
    }
}