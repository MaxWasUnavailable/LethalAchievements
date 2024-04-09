namespace LethalAchievements.Config.Conditions;

/// <summary>
///     Always succeeds.
/// </summary>
public class AlwaysCondition : ICondition
{
    /// <inheritdoc />
    public bool Evaluate(in Context context)
    {
        return true;
    }
}