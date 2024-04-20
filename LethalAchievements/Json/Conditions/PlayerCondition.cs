using BepInEx.Logging;
using LethalAchievements.Config;
using LethalAchievements.Config.Predicates;
using LethalAchievements.Config.Serialization;
using Newtonsoft.Json;

namespace LethalAchievements.Json.Conditions;

/// <summary>
///     Checks the state of a player.
/// </summary>
/// <remarks>
///     Which player to check is determined by the context, but it is usually the local player.
/// </remarks>
[JsonConverter(typeof(TransparentConverter<PlayerCondition, PlayerPredicate>))]
public class PlayerCondition : ICondition
{
    public PlayerPredicate Predicate;

    /// <summary>
    ///     Creates a new player condition.
    /// </summary>
    /// <param name="predicate"> The predicate to check. </param>
    public PlayerCondition(PlayerPredicate predicate)
    {
        Predicate = predicate;
    }

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