using LethalAchievements.Config.Serialization;
using static LethalAchievements.Config.ConditionHelper;

namespace LethalAchievements.Config.Conditions;

public class QuotaCondition : ICondition
{
    public IntRange? Current { get; }
    public IntRange? Fulfilled { get; }
    public IntRange? CompletedCount { get; }
    public IntRange? DaysUntilDeadline { get; }
    public bool? Reached { get; }
    
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