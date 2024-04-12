using LethalAchievements.Config.Serialization;
using static LethalAchievements.Config.ConditionHelper;

namespace LethalAchievements.Config.Conditions;

/// <summary>
///     Checks various things about the current profit quota.
/// </summary>
public class QuotaCondition : ICondition 
{
    /// <summary>
    ///     Checks the current quota.
    /// </summary>
    public IntRange? Current;

    /// <summary>
    ///     Checks the fulfillment of the current quota.
    /// </summary>
    public IntRange? Fulfilled;
    
    /// <summary>
    ///     Checks the number of quotas completed.
    /// </summary>
    public IntRange? CompletedCount;
    
    /// <summary>
    ///     Checks the days until the deadline.
    /// </summary>
    public IntRange? DaysUntilDeadline;

    /// <summary>
    ///     Checks the company current buy rate, where 1 is 100%.
    /// </summary>
    public FloatRange? CompanyBuyingRate;
    
    /// <summary>
    ///     Checks if the current quota has been reached.
    /// </summary>
    public bool? Reached;
    
    /// <inheritdoc />
    public bool Evaluate(in Context context)
    {
        var timeOfDay = TimeOfDay.Instance;
        
        return All(
            Predicate(timeOfDay.profitQuota, Current),
            Predicate(timeOfDay.quotaFulfilled, Fulfilled),
            Predicate(timeOfDay.timesFulfilledQuota, CompletedCount),
            Predicate(timeOfDay.daysUntilDeadline, DaysUntilDeadline),
            Predicate(StartOfRound.Instance.companyBuyingRate, CompanyBuyingRate),
            Predicate(timeOfDay.reachedQuota, Reached)
        );
    }
}