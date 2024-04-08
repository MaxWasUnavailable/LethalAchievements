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
    public LevelWeatherType[]? Types;

    /// <inheritdoc />
    public bool Evaluate(in Context context)
    {
        return Contains(TimeOfDay.Instance.currentLevelWeather, Types);
    }
}