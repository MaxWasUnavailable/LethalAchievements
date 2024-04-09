﻿using BepInEx.Logging;
using GameNetcodeStuff;
using static LethalAchievements.Config.ConditionHelper;

namespace LethalAchievements.Config.Predicates;

/// <summary>
///     Checks the state of a player.
/// </summary>
public class PlayerPredicate
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
    ///     Checks if the player is in free camera mode.
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
    ///     Checks if the player is falling, but not from jumping.
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
    
    public bool Check(PlayerControllerB player)
    {
        return All(
            Matches(player.isInsideFactory, InFacility),
            Matches(player.bleedingHeavily, BleedingHeavily),
            Matches(player.isExhausted, Exhausted),
            Matches(player.isJumping, Jumping),
            Matches(player.isSprinting, Sprinting),
            Matches(player.isSidling, Sidling),
            Matches(player.isUnderwater, Underwater),
            Matches(player.isWalking, Walking),
            Matches(player.isClimbingLadder, ClimbingLadder),
            Matches(player.isFreeCamera, FreeCamera),
            Matches(player.isHoldingInteract, HoldingInteract),
            Matches(player.isHoldingObject, HoldingObject),
            Matches(player.isInElevator, OnShip),
            Matches(player.isPlayerAlone, Alone),
            Matches(player.isPlayerDead, Dead),
            Matches(player.isTypingChat, TypingChat),
            Matches(player.isFallingFromJump, FallingFromJump),
            Matches(player.isFallingNoJump, FallingNoJump),
            Matches(player.isGroundedOnServer, Grounded),
            Matches(player.IsHost, Host)
        );
    }
}