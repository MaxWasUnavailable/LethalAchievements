namespace LethalAchievements.Config.Conditions;

/// <summary>
///     An achievement condition that always returns false.
/// </summary>
public class Never : ICondition
{
    /// <inheritdoc />
    public bool Evaluate()
    {
        return false;
    }
}