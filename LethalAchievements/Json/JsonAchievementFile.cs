using System.IO;
using System.Linq;
using LethalAchievements.Config.Serialization;
using LethalModDataLib.Enums;
using Newtonsoft.Json;
using UnityEngine;

namespace LethalAchievements.Config;

internal class JsonAchievementFile
{
    public string? Name;
    
    [JsonRequired]
    public string DisplayText;
    
    public string? Description;

    public bool Global = false;

    [JsonProperty("icon")]
    public string? RelativeIconPath;

    [JsonRequired]
    [JsonConverter(typeof(OneOrMultipleConverter))]
    public Criterion[] Criteria;
    
    public bool Debug = false;
    
    public JsonAchievement ToAchievement(string filePath)
    {
        // PluginName
        // -> Achievements
        // -> -> Achievement1.json
        var pluginDir = Path.GetDirectoryName(Path.GetDirectoryName(filePath))!;
        var pluginName = Path.GetFileName(pluginDir);
        var name = Name ?? Path.GetFileNameWithoutExtension(filePath);
        
        return new JsonAchievement(pluginName, name, Criteria) {
            DisplayText = DisplayText,
            Description = Description,
            Icon = LoadIcon(filePath),
            SaveLocation = Global ? SaveLocation.GeneralSave : SaveLocation.CurrentSave,
            Debug = Debug
        };
    }

    private Sprite? LoadIcon(string filePath)
    {
        var relativePath = RelativeIconPath ?? Path.GetFileNameWithoutExtension(filePath) + ".png";
        
        var path = Path.Combine(Path.GetDirectoryName(filePath)!, relativePath);
        if (!File.Exists(path))
        {
            if (RelativeIconPath == null)
                return null; // handle gracefully if user didn't specify an icon explicitly
            
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