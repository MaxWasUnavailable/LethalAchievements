using System;
using LethalAchievements.Interfaces;
using LethalModDataLib.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LethalAchievements.UI;

internal class AchievementEntry : MonoBehaviour
{
    private TMP_Text description;
    private TMP_Text goal;
    private TMP_Text progress;
    private TMP_Text target;
    private Transform progressBar;

    internal void Init(IAchievement achievement)
    {
        var isGlobal = achievement.SaveLocation == SaveLocation.GeneralSave;
        
        transform.Find("Name").GetComponent<TMP_Text>().text =  achievement.Name.Truncate(45);
        description = transform.Find("Description").GetComponent<TMP_Text>();
        if (!achievement.IsHidden || achievement.IsAchieved)
        {
           // If no description provided, use display text instead
           var descriptionText = achievement.Description ?? achievement.DisplayText;
           description.text = String.Concat("> ", descriptionText).Truncate(170); 
        }
        else
        {
            description.fontStyle = FontStyles.Italic;
            description.color = new Color32(255, 50, 0, 100);
            description.text = "Hidden";
        }
        
        goal = transform.Find("ProgressionObject/Goal").GetComponent<TMP_Text>();
        progress = transform.Find("ProgressionObject/Progress").GetComponent<TMP_Text>();
        target = transform.Find("ProgressionObject/Target").GetComponent<TMP_Text>();
        progressBar = transform.Find("ProgressBar");
        transform.Find("IconContainer/Global").gameObject.SetActive(isGlobal);

        SetProgressDisplay(achievement);
        
        // Set achievement image to one found with {name}.png
        // Current directory for now, CustomSounds has logic for all mods that have a folder named "Custom sounds" 
        // TODO: Make class/method to allow devs to choose how they load icons
        try
        {
            var iconPath = $"./{achievement}.png";
            var rawData = System.IO.File.ReadAllBytes(iconPath);
            var iconTexture = new Texture2D(512, 512);
            iconTexture.LoadImage(rawData);

            LethalAchievements.Logger?.LogDebug($"Icon found for {achievement.Name}");
            // Create sprite with texture
            transform.Find("IconContainer/Icon").GetComponent<Image>().sprite = Sprite.Create(iconTexture,
                new Rect(0, 0, iconTexture.width, iconTexture.height), Vector2.zero);
        }
        catch
        {
            LethalAchievements.Logger?.LogDebug($"No icon found for {achievement.Name}! - Using default icon");
        }
    }

    private void SetProgressDisplay(IAchievement achievement)
    {
        if (UpdateProgress(achievement))
        {
            goal.text = achievement.ProgressText.Truncate(60);
            target.text = achievement.ProgressTarget.ToString().Truncate(5);
        }
        else
        {
            LethalAchievements.Logger?.LogDebug($"{achievement.Name} is not progress-based, excluding progression section."); 
            
            goal.gameObject.SetActive(false);
            progress.gameObject.SetActive(false);
            target.gameObject.SetActive(false);
            
            transform.Find("GoalDivide").gameObject.SetActive(false);
            transform.Find("ProgressionObject/Divide").gameObject.SetActive(false);
            progressBar.localScale = achievement.IsAchieved ? new Vector3(1, 1, 1) : new Vector3(0, 1, 1);
        }
    }

    internal bool UpdateProgress(IAchievement achievement)
    {
        if (!achievement.GetProgress().HasValue || !achievement.ProgressTarget.HasValue) return false;
        
        LethalAchievements.Logger?.LogDebug($"{achievement.Name} is progress-based, including progression section.");
        progress.text = achievement.GetProgress().ToString().Truncate(5);
        
        // Logic to set length of progress bar
        var barLength = Mathf.Clamp(achievement.GetProgress()!.Value / achievement.ProgressTarget.Value, 0f, 1f);
        progressBar.localScale = new Vector3(barLength, 1f, 1f);
        return true;
    }
}