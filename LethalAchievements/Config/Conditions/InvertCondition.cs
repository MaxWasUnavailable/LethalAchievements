using LethalAchievements.Config.Serialization;
using Newtonsoft.Json;

namespace LethalAchievements.Config.Conditions;

/// <summary>
///     Inverts the result of another condition.
/// </summary>
[JsonConverter(typeof(TransparentConverter<InvertCondition, TaggedCondition>))]
public class InvertCondition : ICondition
{
    /// <summary>
    ///     The condition to invert.
    /// </summary>
    public TaggedCondition Condition { get; }
    
    /// <summary>
    ///     Creates a new <see cref="InvertCondition"/> with an inner condition.
    /// </summary>
    public InvertCondition(TaggedCondition condition)
    {
        Condition = condition;
    }

    /// <inheritdoc />
    public bool Evaluate(in Context context)
    {
        return !Condition.Value.Evaluate(context);
    }
}