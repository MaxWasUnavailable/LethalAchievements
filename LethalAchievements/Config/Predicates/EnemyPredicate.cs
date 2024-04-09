using LethalAchievements.Config.Serialization;
using Newtonsoft.Json;
using static LethalAchievements.Config.ConditionHelper;

namespace LethalAchievements.Config.Predicates;

public class EnemyPredicate : IPredicate<EnemyAI>
{
    public bool? Outside;
    
    [JsonConverter(typeof(OneOrMultipleConverter<string>))]
    public string[]? Name;
    
    public bool Check(EnemyAI enemy) {
        var type = enemy.enemyType;
        
        return All(
            Matches(enemy.isOutside, Outside),
            Contains(type.name, Name)
        );
    }
}