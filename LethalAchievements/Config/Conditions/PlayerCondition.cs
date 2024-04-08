using BepInEx.Logging;
using static LethalAchievements.Config.Condition;

namespace LethalAchievements.Config.Conditions;

/// <summary>
///     Checks the state of a player.
/// </summary>
/// <remarks>
///     Which player to check is determined by the context, but it is usually the local player.
/// </remarks>
public class PlayerCondition : ICondition
{
    /// <summary>
    ///     Checks if the player is inside the facility.
    /// </summary>
    public bool? InFacility;
    
    /// <summary>
    ///     Checks if the player is bleeding after taking critical damage.
    /// </summary>
    public bool? BleedingHeavily;
    
    /// <summary>
    ///     Checks if the player is out of stamina.
    /// </summary>
    public bool? Exhausted;
    
    /// <summary>
    ///     Checks if the player has jumped in the last 0.25 seconds.
    /// </summary>
    public bool? Jumping;
    
    /// <summary>
    ///     Checks if the player is sprinting.
    /// </summary>
    public bool? Sprinting;
    
    /// <summary>
    ///     Checks if the player is walking sideways.
    /// </summary>
    public bool? Sidling;
    
    /// <summary>
    ///     Checks if the player is underwater.
    /// </summary>
    public bool? Underwater;
    
    /// <summary>
    ///     Checks if the player is walking OR sprinting.
    /// </summary>
    public bool? Walking;
    
    /// <summary>
    ///     Checks if the player is climbing a ladder.
    /// </summary>
    public bool? ClimbingLadder;
    
    /// <summary>
    ///     Checks if the player is in cinematic camera mode.
    /// </summary>
    public bool? FreeCamera;
    
    /// <summary>
    ///     Checks if the player is holding the interact key.
    /// </summary>
    public bool? HoldingInteract;
    
    /// <summary>
    ///     Checks if the player is holding an object.
    /// </summary>
    public bool? HoldingObject;
    
    /// <summary>
    ///     Checks if the player is on the ship, but not in it, i.e. on the roof or the balcony.
    /// </summary>
    public bool? OnShip;
    
    /// <summary>
    ///     Checks if the player is far from other players and is not talking on the walkie-talkie.
    /// </summary>
    public bool? Alone;
    
    /// <summary>
    ///     Checks if the player is dead.
    /// </summary>
    public bool? Dead;
    
    /// <summary>
    ///     Checks if the player is typing in the chat.
    /// </summary>
    public bool? TypingChat;
    
    /// <summary>
    ///     Checks if the player is falling after jumping.
    /// </summary>
    public bool? FallingFromJump;
    
    /// <summary>
    ///     Checks if the player is falling without jumping.
    /// </summary>
    public bool? FallingNoJump;
    
    /// <summary>
    ///     Checks if the player is grounded.
    /// </summary>
    public bool? Grounded;
    
    /// <summary>
    ///     Checks if the player is the host of the game.
    /// </summary>
    public bool? Host;

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
            Matches(TypingChat, player.isTypingChat),
            Matches(FallingFromJump, player.isFallingFromJump),
            Matches(FallingNoJump, player.isFallingNoJump),
            Matches(Grounded, player.isGroundedOnServer),
            Matches(Host, player.IsHost)
        );
    }
}