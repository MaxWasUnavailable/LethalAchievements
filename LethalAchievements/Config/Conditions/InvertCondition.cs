using Newtonsoft.Json;

namespace LethalAchievements.Config.Conditions;

/// <summary>
///     Inverts the result of another condition.
/// </summary>
public class InvertCondition : ICondition
{
    [JsonRequired]
    public ICondition Condition;
    
    /// <inheritdoc />
    public bool Evaluate(in Context context)
    {
        return !Condition.Evaluate(in context);
    }
}