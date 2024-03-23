using GameNetcodeStuff;
using LethalAchievements.Base;
using LethalEventsLib.Events;
using UnityEngine;

namespace LethalAchievements.Achievements;

public class TestAchievement : BaseAchievement
{
    public override string Name { get; set; } = "Test Achievement";
    public override string? ShortDescription { get; set; } = "This is a test achievement.";
    public override string? LongDescription { get; set; } = "This is a test achievement. It will be achieved when the player jumps.";
    public override Sprite? Icon { get; set; } = null;

    public override void Initialize()
    {
        PlayerEvents.PreJump_performedEvent += OnPlayerJump;
    }

    public override void Uninitialize()
    {
        PlayerEvents.PreJump_performedEvent -= OnPlayerJump;
    }


    private void OnPlayerJump(ref bool cancel, ref PlayerControllerB player)
    {
        Complete();
    }
}