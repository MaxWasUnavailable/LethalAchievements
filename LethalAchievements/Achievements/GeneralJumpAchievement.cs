using GameNetcodeStuff;
using LethalAchievements.Base;
using LethalEventsLib.Events;
using LethalModDataLib.Enums;
using UnityEngine;

namespace LethalAchievements.Achievements;

/// <summary>
///     A test achievement that will be achieved when the player jumps. Saved in the general save file.
/// </summary>
public class GeneralJumpAchievement : BaseAchievement
{
    /// <inheritdoc />
    public override string Name { get; set; } = "Baby's First Jump (General)";
    
    /// <inheritdoc />
    public override string? DisplayText { get; set; } = "Wow, you actually jumped!";
    
    /// <inheritdoc />
    public override string? Description { get; set; } =
        "This is a test achievement. It will be achieved when the player jumps.";

    /// <inheritdoc />
    public override Sprite? Icon { get; set; } = null;

    /// <inheritdoc />
    public override SaveLocation SaveLocation { get; set; } = SaveLocation.GeneralSave;

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