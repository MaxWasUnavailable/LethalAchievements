using LethalAchievements.Config.Serialization;
using Newtonsoft.Json;
using static LethalAchievements.Config.ConditionHelper;

namespace LethalAchievements.Config.Conditions;

/// <summary>
///     Checks if the current weather matches any of the specified weather types.
/// </summary>
public class WeatherCondition : ICondition
{
    /// <summary>
    ///     The weather types to check for.
    /// </summary>
    [JsonConverter(typeof(OneOrMultipleConverter<LevelWeatherType>))]
    public LevelWeatherType[]? Type;

    /// <inheritdoc />
    public bool Evaluate(in Context context)
    {
        return Contains(TimeOfDay.Instance.currentLevelWeather, Type);
    }
}