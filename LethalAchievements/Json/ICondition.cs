namespace LethalAchievements.Json;

/// <summary>
///     A condition.
/// </summary>
public interface ICondition
{
    /// <summary>
    ///     Evaluates the condition.
    /// </summary>
    bool Evaluate(in Context context);
}