using GameNetcodeStuff;
using LethalAchievements.Config.Serialization;
using Newtonsoft.Json;
using static LethalAchievements.Config.ConditionHelper;

namespace LethalAchievements.Config.Predicates;

/// <summary>
///     Checks the state of a player.
/// </summary>
public class PlayerPredicate : IPredicate<PlayerControllerB>
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
    ///     Equivalent to <c>HeldItemPredicate = { }</c> (or <c>"held_item": {}</c> in JSON)
    /// </summary>
    public bool? HoldingItem;
    
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
    
    /// <summary>
    ///     Checks the player's health (max is 100).
    /// </summary>
    public IntRange? Health;
    
    /// <summary>
    ///     Checks the player's insanity level.
    ///     The instanity level increases when a player is <see cref="Alone"/>.
    /// </summary>
    public FloatRange? Insanity;

    /// <summary>
    ///     Checks the player's drunkness level (gained from taking TzP).
    ///     At 0.15 the player's voice starts to increase in pitch.
    ///     At 0.4 the screen starts to blur.
    /// </summary>
    public FloatRange? Drunkness;
    
    /// <summary>
    ///     Checks the carry weight of the player, in pounds.
    /// </summary>
    public IntRange? Weight;

    /// <summary>
    ///     Checks the player's held item. If the player is not holding an item, this will automatically fail.
    /// </summary>
    /// <seealso cref="ItemPredicate"/>
    public ItemPredicate? HeldItem;

    /// <summary>
    ///     Checks the player's experience level.<br/>
    ///     0 - Intern<br/>
    ///     1 - Part-timer<br/>
    ///     2 - Employee<br/>
    ///     3 - Leader<br/>
    ///     4 - Boss<br/>
    ///     5 - VIP Employee
    /// </summary>
    public IntRange? Level;

    /// <summary>
    ///    Checks for items in the player's inventory. All predicates must match at least one item in the inventory.
    /// </summary>
    /// <seealso cref="ItemPredicate"/>
    public ItemPredicate[]? Inventory;
    
    /// <summary>
    ///     Checks if the player matches all of the specified conditions.
    /// </summary>
    public bool Check(PlayerControllerB player)
    {
        return All(
            Predicate(player.health, Health),
            Predicate(player.insanityLevel, Insanity),
            Predicate(player.drunkness, Drunkness),
            Predicate(ConversionHelper.ToPounds(player.carryWeight), Weight),
            Predicate(player.currentlyHeldObjectServer, HeldItem),
            Predicate(player.playerLevelNumber, Level),
            Predicate(player.ItemSlots, Inventory),
            Predicate(player.isInsideFactory, InFacility),
            Predicate(player.bleedingHeavily, BleedingHeavily),
            Predicate(player.isExhausted, Exhausted),
            Predicate(player.isJumping, Jumping),
            Predicate(player.isSprinting, Sprinting),
            Predicate(player.isSidling, Sidling),
            Predicate(player.isUnderwater, Underwater),
            Predicate(player.isWalking, Walking),
            Predicate(player.isClimbingLadder, ClimbingLadder),
            Predicate(player.isFreeCamera, FreeCamera),
            Predicate(player.isHoldingInteract, HoldingInteract),
            Predicate(player.isHoldingObject, HoldingItem),
            Predicate(player.isInElevator, OnShip),
            Predicate(player.isPlayerAlone, Alone),
            Predicate(player.isPlayerDead, Dead),
            Predicate(player.isTypingChat, TypingChat),
            Predicate(player.isFallingFromJump, FallingFromJump),
            Predicate(player.isFallingNoJump, FallingNoJump),
            Predicate(player.isGroundedOnServer, Grounded),
            Predicate(player.IsHost, Host)
        );
    }
}