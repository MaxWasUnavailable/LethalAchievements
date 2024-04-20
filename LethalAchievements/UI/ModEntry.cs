using System.Collections.Generic;
using System.IO;
using System.Linq;
using LethalAchievements.Helpers;
using LethalAchievements.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LethalAchievements.UI;

internal class ModEntry : MonoBehaviour
{
    private GameObject Divider;
    private TMP_Text Name;

    private List<ModEntry> AllMods;
    internal Dictionary<IAchievement, AchievementEntry> AchievementEntries = [];

    private bool isExpanded = false;

    internal void Init(string modName, List<IAchievement> achievements, List<ModEntry> mods, Transform achievementList)
    {
        // Used to display mod name
        Name = transform.Find("Name").GetComponent<TMP_Text>();
        Name.text = modName.Truncate(30);
        Name.gameObject.SetActive(false);
        
        // Employs seamless merging with window
        Divider = transform.Find("Divide").gameObject;
        Divider.gameObject.SetActive(false);
        
        transform.GetComponent<Button>().onClick.AddListener(() => OnClick());
        
        // Set mod image to one found with {name}.png
        // Make a class for different types of icon loadings etc etc
        // Current directory for now, CustomSounds has logic for all mods that have a folder named "Custom sounds" 
        try
        {
            var iconPath = $"./{modName}.png";
            var rawData = File.ReadAllBytes(iconPath);
            var iconTexture = new Texture2D(64, 64);
            iconTexture.LoadImage(rawData);

            // Create sprite with texture
            transform.Find("Icon").GetComponent<Image>().sprite = Sprite.Create(iconTexture,
                new Rect(0, 0, iconTexture.width, iconTexture.height), Vector2.zero);
        }
        catch
        {
            LethalAchievements.Logger?.LogDebug($"No icon found for {modName}! - Using default icon.");
        }

        AllMods = mods;
        var achievementTemplate = achievementList.Find("Achievement").gameObject;
        achievementTemplate.SetActive(false);
        foreach (var achievement in achievements)
        {
            var achievementObj = Instantiate(achievementTemplate, achievementList);
            achievementObj.SetActive(false);
            var achievementEntry = achievementObj.AddComponent<AchievementEntry>();
            achievementEntry.Init(achievement);
            AchievementEntries.Add(achievement, achievementEntry);
        }
    }

    private void OnClick()
    {
        HUDController.ClickSFX.Play();
        // Sets any mods not clicked to not be displayed
        if(!isExpanded) Toggle(!isExpanded);
    }
    private void Toggle(bool isActive)
    {
        if (isActive)
            foreach (var mod in AllMods.Where(mod => mod != this)) mod.Toggle(false);
        
        foreach (var achievement in AchievementEntries) achievement.Value.gameObject.SetActive(isActive);
        isExpanded = isActive;
        Name.gameObject.SetActive(isActive);
        Divider.gameObject.SetActive(isActive);
    }
}