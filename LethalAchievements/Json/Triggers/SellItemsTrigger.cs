using System;
using LethalAchievements.Config.Predicates;
using LethalAchievements.Config.Serialization;
using LethalAchievements.Events;
using Newtonsoft.Json;
using static LethalAchievements.Config.ConditionHelper;

namespace LethalAchievements.Config.Triggers;

/// <summary>
///     Triggered when items are sold at the company, specifically when the popup appears.
/// </summary>
public class SellItemsTrigger : ITrigger
{
    /// <summary>
    ///     Checks the profit that was made. This includes the current buying rate.
    /// </summary>
    public IntRange? Profit;

    /// <summary>
    ///     Checks the items that were sold. All of the predicates must match at least one of the sold items.
    /// </summary>
    [JsonConverter(typeof(OneOrMultipleConverter))]
    public ItemPredicate[]? Items;
    
    /// <inheritdoc />
    public event Action<Context>? OnTriggered;

    /// <inheritdoc />
    public void Initialize()
    {
        DepositDeskEvents.OnItemsSold += OnItemsSold;
    }

    /// <inheritdoc />
    public void Uninitialize()
    {
        DepositDeskEvents.OnItemsSold -= OnItemsSold;
    }

    private void OnItemsSold(DepositItemsDesk desk, in ItemsSoldContext context)
    {
        if (!Matches(context.Profit, Profit)) return;
        if (!Matches(context.Items, Items)) return;
        
        OnTriggered?.Invoke(Context.Default());
    }
}