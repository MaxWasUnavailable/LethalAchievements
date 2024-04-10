using BepInEx.Logging;
using LethalAchievements.Config.Predicates;
using LethalAchievements.Config.Serialization;
using Newtonsoft.Json;
using static LethalAchievements.Config.ConditionHelper;

namespace LethalAchievements.Config.Conditions;

/// <summary>
///     Checks the state of a player.
/// </summary>
/// <remarks>
///     Which player to check is determined by the context, but it is usually the local player.
/// </remarks>
[JsonConverter(typeof(TransparentConverter))]
public class PlayerCondition : ICondition
{
    public PlayerPredicate Predicate;
    
    /// <inheritdoc />
    public bool Evaluate(in Context context)
    {
        var player = context.Player;
        if (player is null)
        {
            context.Achievement.Log("Player condition cannot be in a context without a player.", LogLevel.Warning);
            return false;
        }

        return Predicate.Check(player);
    }
}