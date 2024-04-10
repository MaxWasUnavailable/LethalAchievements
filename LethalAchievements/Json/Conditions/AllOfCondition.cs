using LethalAchievements.Config.Serialization;
using Newtonsoft.Json;

namespace LethalAchievements.Config.Conditions;

/// <summary>
///     Evaluates to true if all of the <see cref="Terms"/> are true.
/// </summary>
public class AllOfCondition : ICondition 
{
    /// <summary>
    ///     The conditions to evaluate.
    /// </summary>
    [JsonRequired]
    [JsonConverter(typeof(InternalTagConverter<ICondition>))]
    public ICondition[] Terms;

    /// <inheritdoc />
    public bool Evaluate(in Context context)
    {
        // cannot use LINQ since 'in' can't be used in lambdas
        foreach (var term in Terms)
        {
            if (!term.Evaluate(in context))
                return false;
        }

        return true;
    }
}