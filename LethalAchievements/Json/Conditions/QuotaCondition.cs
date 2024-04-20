using LethalAchievements.Config;
using LethalAchievements.Config.Serialization;
using static LethalAchievements.Config.ConditionHelper;

namespace LethalAchievements.Json.Conditions;

/// <summary>
///     Checks various things about the current profit quota.
/// </summary>
public class QuotaCondition : ICondition
{
    /// <summary>
    ///     Checks the number of quotas completed.
    /// </summary>
    public IntRange? CompletedCount;

    /// <summary>
    ///     Checks the current quota.
    /// </summary>
    public IntRange? Current;

    /// <summary>
    ///     Checks the days until the deadline.
    /// </summary>
    public IntRange? DaysUntilDeadline;

    /// <summary>
    ///     Checks the fulfillment of the current quota.
    /// </summary>
    public IntRange? Fulfilled;

    /// <summary>
    ///     Checks if the current quota has been reached.
    /// </summary>
    public bool? Reached;

    /// <inheritdoc />
    public bool Evaluate(in Context context)
    {
        var timeOfDay = TimeOfDay.Instance;

        return All(
            Matches(timeOfDay.profitQuota, Current),
            Matches(timeOfDay.quotaFulfilled, Fulfilled),
            Matches(timeOfDay.timesFulfilledQuota, CompletedCount),
            Matches(timeOfDay.daysUntilDeadline, DaysUntilDeadline),
            Matches(timeOfDay.reachedQuota, Reached)
        );
    }
}