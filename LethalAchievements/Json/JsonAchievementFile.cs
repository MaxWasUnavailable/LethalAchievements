using System.IO;
using System.Linq;
using LethalAchievements.Config.Serialization;
using LethalModDataLib.Enums;
using Newtonsoft.Json;
using UnityEngine;

namespace LethalAchievements.Config;

internal class JsonAchievementFile
{
    /// <summary>
    ///     The name of the achievement, optional.
    ///     Defaults to the name of the file.
    /// </summary>
    public string? Name;

    /// <summary>
    ///     The display text of the achievement, optional.
    /// </summary>
    /// <remarks> Needs to be below ~100 characters. Used for tip popup type. </remarks>
    public string? DisplayText;

    /// <summary>
    ///     The description of the achievement, optional.
    ///     Used instead of the DisplayText for the achievement menu (if specificed).
    /// </summary>
    public string? Description;

    /// <summary>
    ///     Whether the achievement is global or per-save.
    ///     Defaults to false.
    /// </summary>
    public bool Global = false;

    /// <summary>
    ///     A path to an image file to use as the icon for the achievement, optional
    ///     The path is relative to the directory of the achievement file.
    ///     Defaults to a similarly named PNG file in the same directory as the achievement file.
    /// </summary>
    [JsonProperty("icon")]
    public string? RelativeIconPath;

    /// <summary>
    ///     The criteria that need to be completed to achieve the achievement.
    /// </summary>
    [JsonRequired]
    [JsonConverter(typeof(OneOrMultipleConverter))]
    public Criterion[] Criteria;
    
    /// <inheritdoc cref="JsonAchievement.Debug"/>
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