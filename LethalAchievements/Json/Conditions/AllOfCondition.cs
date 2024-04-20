using LethalAchievements.Json.Serialization;
using Newtonsoft.Json;

namespace LethalAchievements.Json.Conditions;

/// <summary>
///     Evaluates to true if all <see cref="Terms" /> are true.
/// </summary>
public class AllOfCondition : ICondition
{
    /// <summary>
    ///     The conditions to evaluate.
    /// </summary>
    [JsonRequired] public TaggedCondition[] Terms;

    /// <inheritdoc />
    public bool Evaluate(in Context context)
    {
        // cannot use LINQ since 'in' can't be used in lambdas
        foreach (var term in Terms)
            if (!term.Value.Evaluate(in context))
                return false;

        return true;
    }
}