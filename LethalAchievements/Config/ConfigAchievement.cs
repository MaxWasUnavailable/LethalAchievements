using System.Collections.Generic;
using System.Linq;
using LethalAchievements.Base;
using UnityEngine;

namespace LethalAchievements.Config;

/// <summary>
///     An achievement loaded from a config file.
/// </summary>
public class ConfigAchievement : BaseAchievement
{
    public override string Name { get; set; }
    public override string? DisplayText { get; set; }
    public override string? Description { get; set; }
    public override Sprite? Icon { get; set; }
    
    public ICondition[]? GlobalConditions;
    public Criterion[] Criteria;

    public override void Initialize()
    {
        foreach (var criterion in Criteria)
        {
            LethalAchievements.Logger!.LogDebug($"Initializing trigger {criterion.Trigger}");
            criterion.Trigger.Initialize();
            criterion.Subscribe(OnCriterionTriggered);
        }
    }

    public override void Uninitialize()
    {
        foreach (var criterion in Criteria)
        {
            criterion.Trigger.Uninitialize();
            criterion.Unsubscribe(OnCriterionTriggered);
        }
    }
    
    private void OnCriterionTriggered(Criterion criterion)
    {
        if (IsAchieved) return;
        if (!EvaluateConditions(GlobalConditions)) return;
        if (!EvaluateConditions(criterion.Conditions)) return;
        
        Complete();
    }

    private static bool EvaluateConditions(IEnumerable<ICondition>? conditions)
    {
        return conditions?.All(condition => condition.Evaluate()) ?? true;
    }
}