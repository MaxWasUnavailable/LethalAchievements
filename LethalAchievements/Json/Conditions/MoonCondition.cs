using LethalAchievements.Config.Serialization;
using Newtonsoft.Json;
using static LethalAchievements.Config.ConditionHelper;

namespace LethalAchievements.Config.Conditions;

/// <summary>
///     Checks properties about the current moon.
/// </summary>
public class MoonCondition : ICondition
{
    /// <summary>
    ///     Checks the current moon's name.
    /// </summary>
    [JsonConverter(typeof(OneOrMultipleConverter))]
    public string[]? Name;

    /// <summary>
    ///     Checks if the lights are on inside the facility.
    /// </summary>
    public bool? InteriorLights;
    
    /// <summary>
    ///     Checks the current moon's weather.
    /// </summary>
    [JsonConverter(typeof(OneOrMultipleConverter))]
    public LevelWeatherType[]? Weather;
    
    /// <summary>
    ///     Checks for parts of the day.
    ///     This corresponds to the icons shown at the top of the HUD.
    ///     On the company moon, this is set to None.
    /// </summary>
    [JsonConverter(typeof(OneOrMultipleConverter))]
    public DayMode[]? DayMode;

    /// <inheritdoc />
    public bool Evaluate(in Context context) {
        var timeOfDay = TimeOfDay.Instance;
        
        return All(
            Contains(timeOfDay.currentLevel.PlanetName, Name),
            Predicate(timeOfDay.insideLighting, InteriorLights),
            Contains(timeOfDay.currentLevelWeather, Weather),
            Contains(timeOfDay.dayMode, DayMode)
        );
    }
}