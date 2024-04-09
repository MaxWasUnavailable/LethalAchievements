using GameNetcodeStuff;

namespace LethalAchievements.Events;

internal readonly struct ItemDepositedContext
{
    public readonly PlayerControllerB Player;
    public readonly GrabbableObject Item;
        
    public ItemDepositedContext(PlayerControllerB player, GrabbableObject item)
    {
        Player = player;
        Item = item;
    }
}