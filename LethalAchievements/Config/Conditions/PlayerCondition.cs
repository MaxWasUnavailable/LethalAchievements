using BepInEx.Logging;
using static LethalAchievements.Config.Condition;

namespace LethalAchievements.Config.Conditions;

/// <summary>
///     An achievement condition that checks the state of the player.
/// </summary>
public class PlayerCondition : ICondition
{
    public bool? InFacility;
    public bool? BleedingHeavily;
    public bool? Exhausted;
    public bool? Jumping;
    public bool? Sprinting;
    public bool? Sidling;
    public bool? Underwater;
    public bool? Walking;
    public bool? ClimbingLadder;
    public bool? FreeCamera;
    public bool? HoldingInteract;
    public bool? HoldingObject;
    public bool? OnShip;
    public bool? Alone;
    public bool? Dead;
    public bool? SpeedCheating;
    public bool? TypingChat;
    public bool? FallingFromJump;
    public bool? FallingNoJump;
    public bool? Grounded;
    public bool? Host;
    public bool? Client;

    /// <inheritdoc />
    public bool Evaluate(in Context context)
    {
        var player = context.Player;
        if (player is null)
        {
            context.Achievement.Log("Player condition cannot be evaluated without a player.", LogLevel.Warning);
            return false;
        }

        return All(
            Matches(InFacility, player.isInsideFactory),
            Matches(BleedingHeavily, player.bleedingHeavily),
            Matches(Exhausted, player.isExhausted),
            Matches(Jumping, player.isJumping),
            Matches(Sprinting, player.isSprinting),
            Matches(Sidling, player.isSidling),
            Matches(Underwater, player.isUnderwater),
            Matches(Walking, player.isWalking),
            Matches(ClimbingLadder, player.isClimbingLadder),
            Matches(FreeCamera, player.isFreeCamera),
            Matches(HoldingInteract, player.isHoldingInteract),
            Matches(HoldingObject, player.isHoldingObject),
            Matches(OnShip, player.isInElevator),
            Matches(Alone, player.isPlayerAlone),
            Matches(Dead, player.isPlayerDead),
            Matches(SpeedCheating, player.isSpeedCheating),
            Matches(TypingChat, player.isTypingChat),
            Matches(FallingFromJump, player.isFallingFromJump),
            Matches(FallingNoJump, player.isFallingNoJump),
            Matches(Grounded, player.isGroundedOnServer),
            Matches(Host, player.IsHost),
            Matches(Client, player.IsClient)
        );
    }
}