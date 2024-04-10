using UnityEngine;

namespace LethalAchievements.Config;

/// <summary>
///     Helper for converting the game's internal units to their UI counterpart.
/// </summary>
public static class ConversionHelper 
{
    /// <summary>
    ///     Converts weight to pounds (lb), as shown in the UI.
    /// </summary>
    public static int ToPounds(float weight) 
    {
        return Mathf.RoundToInt(Mathf.Clamp(weight - 1f, 0.0f, 100f) * 105f);
    }
}