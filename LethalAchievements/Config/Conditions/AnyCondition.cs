using System.Linq;
using LethalAchievements.Config.Serialization;
using Newtonsoft.Json;

namespace LethalAchievements.Config.Conditions;

/// <summary>
///     Evaluates to true if any of the <see cref="Conditions"/> are true.
/// </summary>
[JsonConverter(typeof(TransparentConverter<AnyCondition, TaggedCondition[]>))]
public class AnyCondition : ICondition
{
    /// <summary>
    ///     The conditions to evaluate.
    /// </summary>
    public TaggedCondition[] Conditions { get; }
    
    /// <summary>
    ///     Creates a new <see cref="AnyCondition"/> with the specified conditions.
    /// </summary>
    public AnyCondition(TaggedCondition[] conditions)
    {
        Conditions = conditions;
    }

    /// <inheritdoc />
    public bool Evaluate(in Context context)
    {
        // cannot use LINQ since 'in' can't be used in lambdas
        foreach (var c in Conditions)
        {
            if (c.Value.Evaluate(in context))
                return true;
        }

        return false;
    }
}