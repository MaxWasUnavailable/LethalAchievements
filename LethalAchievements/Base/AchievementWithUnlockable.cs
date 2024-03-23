using UnityEngine;

namespace LethalAchievements.Base;

public abstract class AchievementWithUnlockable : BaseAchievement
{
    // Kept abstract for modders to implement
    public abstract override string Name { get; set; }
    public abstract override string? ShortDescription { get; set; }
    public abstract override string? LongDescription { get; set; }
    public abstract override Sprite? Icon { get; set; }
    public abstract override void Initialize();
    public abstract override void Uninitialize();

    protected virtual void AddUnlockable()
    {
        throw new System.NotImplementedException();
    }

    protected override void Complete()
    {
        base.Complete();
        AddUnlockable();
    }
}