using System;
using System.Linq;
using LethalAchievements.Config.Predicates;
using LethalAchievements.Config.Serialization;
using LethalAchievements.Events;
using Newtonsoft.Json;
using static LethalAchievements.Config.ConditionHelper;

namespace LethalAchievements.Config.Triggers;

/// <summary>
///     Triggered when the ship enters orbit after leaving a moon.
/// </summary>
public class EnterOrbitTrigger : ITrigger
{
    /// <summary>
    ///     Checks if anyone survived.
    /// </summary>
    public bool? NoSurvivors;   
    
    /// <summary>
    ///     Checks the amount of scrap collected.
    ///     This is <b>not</b> set to 0 when there are no survivors.
    /// </summary>
    public IntRange? ScrapCollected;

    public IntRange? ScrapMissed;

    /// <summary>
    ///     Checks the player stats that are displayed on the performance report.
    ///     This is checked against all players, not just the local one.
    /// </summary>
    [JsonConverter(typeof(OneOrMultipleConverter))]
    public EndPlayerStatsPredicate[]? PlayerStats;
    
    /// <inheritdoc />
    public event Action<Context>? OnTriggered;

    /// <inheritdoc />
    public void Initialize()
    {
        LobbyEvents.OnEnteredOrbit += OnEnteredOrbit;
    }

    /// <inheritdoc />
    public void Uninitialize()
    {
        LobbyEvents.OnEnteredOrbit -= OnEnteredOrbit;
    }
    
    private void OnEnteredOrbit()
    {
        var startOfRound = StartOfRound.Instance;
        
        if (!Matches(startOfRound.allPlayersDead, NoSurvivors)) return;
        
        var roundManager = RoundManager.Instance;
        var scrapCollected = roundManager.scrapCollectedInLevel;
        if (!Matches(scrapCollected, ScrapCollected)) return;
        if (!Matches((int) roundManager.totalScrapValueInLevel - scrapCollected, ScrapMissed)) return;

        var indexedStats = startOfRound.gameStats.allPlayerStats.Select((s, i) => (s, i)).ToArray();
        if (!Matches(indexedStats, PlayerStats)) return;
        
        OnTriggered?.Invoke(Context.Default());
    }
}

public enum EndPlayerNote
{
    MostParanoid,
    MostProfitable,
    Laziest,
    SustainedMostInjuries
}

public class EndPlayerStatsPredicate : IPredicate<(PlayerStats, int)>
{
    [JsonConverter(typeof(OneOrMultipleConverter))]
    public EndPlayerNote[]? Note;

    public IntRange? ProfitMade;
    
    public IntRange? TurnAmount;

    public IntRange? StepsTaken;
    
    public IntRange? DamageTaken;
    
    public IntRange? Jumps;
    
    public PlayerPredicate? Player;

    /// <inheritdoc />
    public bool Check((PlayerStats, int) value)
    {
        var (stats, index) = value;

        if (Note != null)
        {
            var notes = stats.playerNotes.Select(StringToNote).ToArray();
            if (Note?.All(check => !notes.Contains(check)) ?? false) return false;
        }
        
        return All(
            Matches(stats.profitable, ProfitMade),
            Matches(stats.turnAmount, TurnAmount),
            Matches(stats.stepsTaken, StepsTaken),
            Matches(stats.damageTaken, DamageTaken),
            Matches(stats.jumps, Jumps),
            Matches(StartOfRound.Instance.allPlayerScripts[index], Player)
        );
    }

    private static EndPlayerNote StringToNote(string str)
    {
        return str switch {
            "The laziest employee." => EndPlayerNote.Laziest,
            "The most paranoid employee." => EndPlayerNote.MostParanoid,
            "Sustained the most injuries." => EndPlayerNote.SustainedMostInjuries,
            "Most profitable" => EndPlayerNote.MostProfitable,
            _ => throw new ArgumentOutOfRangeException(nameof(str), str, null)
        };
    }
}