using LethalAchievements.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LethalAchievements.UI;

internal class AchievementEntry : MonoBehaviour
{
    internal void Init(IAchievement achievement)
    {
        
        transform.Find("Name").GetComponent<TMP_Text>().text = achievement.DisplayText.Truncate(45);
        // transform.Find("Description").GetComponent<TMP_Text>().text = String.Concat("> ", description).Truncate(170);
        // transform.Find("ProgressionObject/Goal").GetComponent<TMP_Text>().text = achievement.goal.Truncate(60);
        // transform.Find("ProgressionObject/Progress").GetComponent<TMP_Text>().text = achievement.progress.ToString().Truncate(5);
        // transform.Find("ProgressionObject/Target").GetComponent<TMP_Text>().text = achievement.target.ToString().Truncate(5);
        
        // Set achievement image to one found with {name}.png
        // Current directory for now, CustomSounds has logic for all mods that have a folder named "Custom sounds" 
        try
        {
            var iconPath = $"./{achievement}.png";
            var rawData = System.IO.File.ReadAllBytes(iconPath);
            var iconTexture = new Texture2D(2, 2);
            iconTexture.LoadImage(rawData);

            // Create sprite with texture
            transform.Find("IconContainer/Icon").GetComponent<Image>().sprite = Sprite.Create(iconTexture,
                new Rect(0, 0, iconTexture.width, iconTexture.height), Vector2.zero);
        }
        catch
        {
            // ignored
        }
    }
}