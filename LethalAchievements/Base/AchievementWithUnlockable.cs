using System;

namespace LethalAchievements.Base;

/// <summary>
///     Base class for achievements that unlock an unlockable.
/// </summary>
public abstract class AchievementWithUnlockable : BaseAchievement
{
    /// <summary>
    ///     Unlocks an unlockable.
    /// </summary>
    /// TODO: Implement this
    protected virtual void AddUnlockable()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    protected override void Complete()
    {
        base.Complete();
        AddUnlockable();
    }
}