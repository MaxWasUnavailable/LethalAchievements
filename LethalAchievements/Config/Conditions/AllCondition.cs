using LethalAchievements.Config.Serialization;
using Newtonsoft.Json;

namespace LethalAchievements.Config.Conditions;

/// <summary>
///     Evaluates to true if all of the <see cref="Conditions"/> are true.
/// </summary>
[JsonConverter(typeof(TransparentConverter<AllCondition, TaggedCondition[]>))]
public class AllCondition : ICondition
{
    /// <summary>
    ///     The conditions to evaluate.
    /// </summary>
    public TaggedCondition[] Conditions { get; }
    
    /// <summary>
    ///     Creates a new <see cref="AllCondition"/> with the specified conditions.
    /// </summary>
    public AllCondition(TaggedCondition[] conditions)
    {
        Conditions = conditions;
    }

    /// <inheritdoc />
    public bool Evaluate(in Context context)
    {
        // cannot use LINQ since 'in' can't be used in lambdas
        foreach (var c in Conditions)
        {
            if (!c.Value.Evaluate(in context))
                return false;
        }

        return true;
    }
}