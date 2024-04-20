using LethalAchievements.Config;

namespace LethalAchievements.Json.Conditions;

/// <summary>
///     Always fails.
/// </summary>
public class NeverCondition : ICondition
{
    /// <inheritdoc />
    public bool Evaluate(in Context context)
    {
        return false;
    }
}