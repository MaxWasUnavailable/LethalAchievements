using GameNetcodeStuff;
using LethalAchievements.Base;
using LethalEventsLib.Events;
using UnityEngine;

namespace LethalAchievements.Achievements;

/// <summary>
///     A test achievement.
/// </summary>
public class TestAchievement : BaseAchievement
{
    /// <inheritdoc />
    public override string Name { get; set; } = "Test Achievement";
    
    /// <inheritdoc />
    public override string? ShortDescription { get; set; } = "This is a test achievement.";
    
    /// <inheritdoc />
    public override string? LongDescription { get; set; } =
        "This is a test achievement. It will be achieved when the player jumps.";

    /// <inheritdoc />
    public override Sprite? Icon { get; set; } = null;
    
    /// <inheritdoc />
    public override void Initialize()
    {
        PlayerEvents.PreJump_performedEvent += OnPlayerJump;
    }

    /// <inheritdoc />
    public override void Uninitialize()
    {
        PlayerEvents.PreJump_performedEvent -= OnPlayerJump;
    }
    
    private void OnPlayerJump(ref bool cancel, ref PlayerControllerB player)
    {
        Complete();
    }
}