using LethalAchievements.Config.Serialization;
using Newtonsoft.Json;
using UnityEngine;
using static LethalAchievements.Config.ConditionHelper;

namespace LethalAchievements.Config.Predicates;

public class ItemPredicate : IPredicate<GrabbableObject> {
    public FloatRange? Weight;
    
    public IntRange? Value;
    
    public bool? IsScrap;
    
    [JsonConverter(typeof(OneOrMultipleConverter<string>))]
    public string[]? Name;
    
    public bool? TwoHanded;
        
    public bool? Conductive;

    public bool Check(GrabbableObject item) {
        Debug.LogError($"Names: {string.Join(", ", Name)}");
        var properties = item.itemProperties;
        
        return All(
            Matches(properties.weight, Weight),
            Matches(item.scrapValue, Value),
            Matches(properties.isScrap, IsScrap),
            Contains(properties.itemName, Name),
            Matches(properties.twoHanded, TwoHanded),
            Matches(properties.isConductiveMetal, Conductive)
        );
    }
}