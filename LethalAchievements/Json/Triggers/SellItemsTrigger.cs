using System;
using LethalAchievements.Config.Predicates;
using LethalAchievements.Config.Serialization;
using LethalAchievements.Events;
using Newtonsoft.Json;
using static LethalAchievements.Config.ConditionHelper;

namespace LethalAchievements.Config.Triggers;

public class SellItemsTrigger : ITrigger
{
    public IntRange? Profit;

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
        if (!Predicate(context.Items, Items)) return;
        
        OnTriggered?.Invoke(Context.Default());
    }
}