using System.IO;
using System.Linq;
using LethalModDataLib.Enums;
using Newtonsoft.Json;
using UnityEngine;

namespace LethalAchievements.Config;

/// <summary>
///     The actual data class being deserialized from .json files.
/// </summary>
public class ConfigAchievementFile
{
    [JsonRequired]
    public string Name;
    
    [JsonRequired]
    public string DisplayText;
    
    public string? Description;

    public bool Global = true;

    [JsonProperty("icon")]
    public string? IconPath;
    
    [JsonProperty("conditions")]
    public ICondition[]? GlobalConditions;

    [JsonRequired]
    public Criterion[] Criteria;
    
    /// <summary>
    ///      Converts this file to a <see cref="ConfigAchievement"/>.
    /// </summary>
    /// <param name="filePath"> The path to the .json file where this data was deserialized from. </param>
    public ConfigAchievement ToAchievement(string filePath)
    {
        LethalAchievements.Logger!.LogDebug($"Criteria: {string.Join(", ", Criteria.Select(c => c.ToString()))}");
        
        return new ConfigAchievement {
            Name = Name,
            DisplayText = DisplayText,
            Description = Description,
            Icon = LoadIcon(filePath),
            SaveLocation = Global ? SaveLocation.GeneralSave : SaveLocation.CurrentSave,
            Criteria = Criteria
        };
    }

    private Sprite? LoadIcon(string filePath)
    {
        if (IconPath == null)
        {
            return null;
        }
        
        var path = Path.Combine(Path.GetDirectoryName(filePath)!, IconPath);
        if (!File.Exists(path))
        {
            throw new FileNotFoundException($"Icon file not found at {path}");
        }
        
        var bytes = File.ReadAllBytes(path);
        var texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);
        
        return Sprite.Create(
            texture,
            new Rect(0, 0, texture.width, texture.height),
            new Vector2(0.5f, 0.5f)
        );
    }
}