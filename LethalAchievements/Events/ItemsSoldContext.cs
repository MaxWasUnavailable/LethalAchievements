namespace LethalAchievements.Events;

internal readonly struct ItemsSoldContext
{
    public readonly int Profit;
    public readonly GrabbableObject[] Items;
        
    public ItemsSoldContext(GrabbableObject[] items, int profit)
    {
        Items = items;
        Profit = profit;
    }
}