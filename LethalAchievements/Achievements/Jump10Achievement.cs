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
    private int _jumpCount = 0;
    
    [ModData(SaveWhen = SaveWhen.OnSave, LoadWhen = LoadWhen.Immediately | LoadWhen.OnLoad, SaveLocation = SaveLocation.CurrentSave)]
    private int JumpCount {
        get => _jumpCount;
        set
        {
            _jumpCount = value;
            LethalAchievements.Logger?.LogInfo($"Jump count is now {_jumpCount}");
            if (_jumpCount >= 10)
            {
                Complete();
            }
        }
    }
    
    /// <inheritdoc />
    public override string Name { get; set; } = "Jump for Joy";
    
    /// <inheritdoc />
    public override string? DisplayText { get; set; } = "You've jumped 10 times!";
    
    /// <inheritdoc />
    public override string? Description { get; set; } =
        "This is a test achievement. It will be achieved when the player jumps 10 times.";

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
        JumpCount++;
    }
}