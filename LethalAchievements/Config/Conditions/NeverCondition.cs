namespace LethalAchievements.Config.Conditions;

/// <summary>
///     An achievement condition that always returns false.
/// </summary>
public class NeverCondition : ICondition
{
    /// <inheritdoc />
    public bool Evaluate(in Context context)
    {
        return false;
    }
}