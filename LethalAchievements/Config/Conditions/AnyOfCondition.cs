using System.Linq;
using LethalAchievements.Config.Serialization;
using Newtonsoft.Json;

namespace LethalAchievements.Config.Conditions;

/// <summary>
///     Evaluates to true if any of the <see cref="Terms"/> are true.
/// </summary>
public class AnyOfCondition : ICondition
{
    /// <summary>
    ///     The conditions to evaluate.
    /// </summary>
    [JsonRequired]
    public TaggedCondition[] Terms;

    /// <inheritdoc />
    public bool Evaluate(in Context context)
    {
        // cannot use LINQ since 'in' can't be used in lambdas
        foreach (var c in Terms)
        {
            if (c.Value.Evaluate(in context))
                return true;
        }

        return false;
    }
}