using GameNetcodeStuff;
using LethalAchievements.Base;
using LethalEventsLib.Events;
using LethalModDataLib.Attributes;
using LethalModDataLib.Enums;
using UnityEngine;

namespace LethalAchievements.Achievements;

/// <summary>
///     A test achievement that will be achieved when the player jumps 10 times.
/// </summary>
public class Jump10Achievement : BaseAchievement
{
    [ModData(SaveWhen = SaveWhen.OnSave, LoadWhen = LoadWhen.OnLoad, SaveLocation = SaveLocation.CurrentSave, ResetWhen = ResetWhen.OnGameOver)]
    private int JumpCount { get; set; }
    
    /// <inheritdoc />
    public override string Name { get; set; } = "Jump for Joy";
    
    /// <inheritdoc />
    public override string? DisplayText { get; set; } = "You've jumped 10 times!";
    
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
        JumpCount++;
        if (JumpCount >= 10)
        {
            Complete();
        }
    }
}