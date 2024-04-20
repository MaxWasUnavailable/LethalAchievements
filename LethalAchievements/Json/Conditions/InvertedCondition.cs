using LethalAchievements.Config;
using LethalAchievements.Json.Serialization;
using Newtonsoft.Json;

namespace LethalAchievements.Json.Conditions;

/// <summary>
///     Inverts the result of another condition.
/// </summary>
public class InvertedCondition : ICondition
{
    /// <summary>
    ///     The condition to invert.
    /// </summary>
    [JsonRequired] public TaggedCondition Term;

    /// <inheritdoc />
    public bool Evaluate(in Context context)
    {
        return !Term.Value.Evaluate(context);
    }
}