using LethalAchievements.Config;
using LethalAchievements.Config.Serialization;
using Newtonsoft.Json;

namespace LethalAchievements.Json.Conditions;

/// <summary>
///     Evaluates to true if any <see cref="Terms" /> are true.
/// </summary>
public class AnyOfCondition : ICondition
{
    /// <summary>
    ///     The conditions to evaluate.
    /// </summary>
    [JsonRequired] public TaggedCondition[] Terms;

    /// <inheritdoc />
    public bool Evaluate(in Context context)
    {
        // cannot use LINQ since 'in' can't be used in lambdas
        foreach (var c in Terms)
            if (c.Value.Evaluate(in context))
                return true;

        return false;
    }
}