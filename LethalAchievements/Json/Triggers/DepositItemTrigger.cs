using System;
using LethalAchievements.Config.Predicates;
using LethalAchievements.Events;
using Newtonsoft.Json;
using static LethalAchievements.Config.ConditionHelper;

namespace LethalAchievements.Config.Triggers;

/// <summary>
///     Triggered when the local players puts an item on the deposit desk at the company moon.
/// </summary>
public class DepositItemTrigger : ITrigger
{
    /// <summary>
    ///     Checks the item that was deposited.
    /// </summary>
    public ItemPredicate? Item;
    
    /// <inheritdoc />
    public event Action<Context>? OnTriggered;

    /// <inheritdoc />
    public void Initialize()
    {
        DepositDeskEvents.OnItemDeposited += OnItemDeposited;
    }

    /// <inheritdoc />
    public void Uninitialize()
    {
        DepositDeskEvents.OnItemDeposited -= OnItemDeposited;
    }
    
    private void OnItemDeposited(DepositItemsDesk desk, in ItemDepositedContext context)
    {
        if (!context.Player.IsOwner) return;
        if (!Predicate(context.Item, Item)) return;
        
        OnTriggered?.Invoke(Context.Default());
    }
}