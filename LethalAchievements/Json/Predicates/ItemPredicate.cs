using LethalAchievements.Config.Serialization;
using Newtonsoft.Json;
using UnityEngine;
using static LethalAchievements.Config.ConditionHelper;

namespace LethalAchievements.Config.Predicates;

/// <summary>
///     Checks properties of an item.
/// </summary>
public class ItemPredicate : IPredicate<GrabbableObject>
{
    /// <summary>
    ///     Checks the weight of the item. Can be a range of values.
    /// </summary>
    public IntRange? Weight;

    /// <summary>
    ///     Checks the sell value of the item. Can be a range of values.
    /// </summary>
    public IntRange? Value;

    /// <summary>
    ///     Checks whether the items spawns in the facility, or is a store-bought one.
    /// </summary>
    public bool? IsScrap;

    /// <summary>
    ///     Checks the name of the item. If you specify multiple, any of them will match.
    /// </summary>
    [JsonConverter(typeof(OneOrMultipleConverter))]
    public string[]? Name;

    /// <summary>
    ///     Checks if the item is two-handed.
    /// </summary>
    public bool? TwoHanded;

    /// <summary>
    ///     Checks if the item can be struck by lightning.
    /// </summary>
    public bool? Conductive;

    /// <inheritdoc />
    public bool Check(GrabbableObject item)
    {
        var properties = item.itemProperties;

        return All(
            Matches(ConversionHelper.ToPounds(properties.weight), Weight),
            Matches(item.scrapValue, Value),
            Matches(properties.isScrap, IsScrap),
            Contains(properties.itemName, Name),
            Matches(properties.twoHanded, TwoHanded),
            Matches(properties.isConductiveMetal, Conductive)
        );
    }
}