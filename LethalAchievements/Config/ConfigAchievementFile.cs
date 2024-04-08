using System.IO;
using System.Linq;
using LethalModDataLib.Enums;
using Newtonsoft.Json;
using UnityEngine;

namespace LethalAchievements.Config;

internal class ConfigAchievementFile
{
    [JsonRequired]
    public string Name;
    
    [JsonRequired]
    public string DisplayText;
    
    public string? Description;

    public bool Global = false;

    [JsonProperty("icon")]
    public string? RelativeIconPath;

    [JsonRequired]
    public Criterion[] Criteria;
    
    public bool Debug = false;
    
    public ConfigAchievement ToAchievement(string filePath)
    {
        return new ConfigAchievement(Name, Criteria) {
            DisplayText = DisplayText,
            Description = Description,
            Icon = LoadIcon(filePath),
            SaveLocation = Global ? SaveLocation.GeneralSave : SaveLocation.CurrentSave,
            Debug = Debug
        };
    }

    private Sprite? LoadIcon(string filePath)
    {
        if (RelativeIconPath == null)
        {
            return null;
        }
        
        var path = Path.Combine(Path.GetDirectoryName(filePath)!, RelativeIconPath);
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