namespace LethalAchievements.Config.Conditions;

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