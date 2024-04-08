using System.Collections.Generic;
using System.Linq;
using BepInEx.Logging;
using LethalAchievements.Base;
using UnityEngine;

namespace LethalAchievements.Config;

/// <summary>
///     An achievement loaded from a config file.
/// </summary>
public class ConfigAchievement : BaseAchievement
{
    /// <inheritdoc />
    public override string Name { get; set; }

    /// <inheritdoc />
    public override string? DisplayText { get; set; }

    /// <inheritdoc />
    public override string? Description { get; set; }

    /// <inheritdoc />
    public override Sprite? Icon { get; set; }

    /// <summary>
    ///     A list of <see cref="Criterion"/> that trigger the achievement.
    ///     If any of the criteria are met, the achievement is achieved.
    /// </summary>
    public List<Criterion> Criteria = new();
    
    
    /// <summary>
    ///    Logs to the console every time the achievement would be achieved (even if it's already achieved).
    ///    Make sure to enable the debug log level in the BepInEx config.
    ///    <br/><br/>
    ///    Note that if the achivemenet is achieved at the start of the game, it will not initialize (might change in the future).
    /// </summary>
    public bool Debug = false;

    /// <summary>
    ///     Creates a new achievement with the given name and criteria.
    /// </summary>
    public ConfigAchievement(string name, params Criterion[] criteria)
    {
        Name = name;
        Criteria.AddRange(criteria);
    }

    /// <inheritdoc />
    public override void Initialize()
    {
        if (Debug) Log("Initialized in debug mode", LogLevel.Debug);
        
        foreach (var criterion in Criteria)
        {
            criterion.Trigger.Initialize();
            criterion.Subscribe(OnCriterionTriggered);
        }
    }

    /// <inheritdoc />
    public override void Uninitialize()
    {
        foreach (var criterion in Criteria)
        {
            criterion.Trigger.Uninitialize();
            criterion.Unsubscribe(OnCriterionTriggered);
        }
    }
    
    private void OnCriterionTriggered(Criterion criterion, Context context)
    {
        if (IsAchieved && !Debug) return;

        context.Achievement = this;
        if (!criterion.Conditions?.All(condition => condition.Evaluate(in context)) ?? false) return;

        if (Debug)
        {
            Log("Achieved", LogLevel.Debug);
        }

        if (!IsAchieved)
        {
            Complete();
        }
    }

    internal void Log(object message, LogLevel level)
    {
        LethalAchievements.Logger?.Log(level, $" [ACHIEVEMENT {Name}] {message}");
    }
}