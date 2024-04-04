using LethalAchievements.Base;
using LethalModDataLib.Attributes;
using LethalModDataLib.Enums;
using UnityEngine;

/// <summary>
///     An example achievement that will be achieved when the player jumps 10 times.
/// </summary>
public class Jump10Achievement : BaseAchievement
{
    /// <summary>
    ///     The number of times the player has jumped.
    /// </summary>
    /// <remarks> This field will be saved and loaded by the ModData library. </remarks>
    [ModData(SaveWhen = SaveWhen.OnSave, LoadWhen = LoadWhen.OnLoad, SaveLocation = SaveLocation.CurrentSave)]
    private int JumpCount { get; set; }
    
    /// <inheritdoc />
    public override string Name { get; set; } = "Jump for Joy";
    
    /// <inheritdoc />
    public override string? DisplayText { get; set; } = "You've jumped 10 times!";

    public override Sprite? Icon { get; set; }

    /// <inheritdoc />
    public override void Initialize()
    {
    }

    /// <inheritdoc />
    public override void Uninitialize()
    {
    }
    
    private void OnPlayerJump()
    {
        JumpCount++;
        if (JumpCount >= 10)
        {
            Complete();
        }
    }
}