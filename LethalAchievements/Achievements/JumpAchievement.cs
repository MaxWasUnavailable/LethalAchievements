using GameNetcodeStuff;
using LethalAchievements.Base;
using LethalEventsLib.Events;
using UnityEngine;

namespace LethalAchievements.Achievements;

/// <summary>
///     A test achievement that will be achieved when the player jumps.
/// </summary>
public class JumpAchievement : BaseAchievement
{
    /// <inheritdoc />
    public override string Name { get; set; } = "Baby's First Jump";
    
    /// <inheritdoc />
    public override string? DisplayText { get; set; } = "Wow, you actually jumped!";
    
    /// <inheritdoc />
    public override void Initialize()
    {
        PlayerEvents.PrePlayerJumpEvent += OnPlayerJump;
    }

    /// <inheritdoc />
    public override void Uninitialize()
    {
        PlayerEvents.PrePlayerJumpEvent -= OnPlayerJump;
    }
    
    private void OnPlayerJump(ref bool cancel, ref PlayerControllerB player)
    {
        Complete();
    }
}