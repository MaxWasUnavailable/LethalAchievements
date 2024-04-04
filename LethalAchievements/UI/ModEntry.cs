using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    private List<AchievementEntry> AchievementEntries = [];

    internal void Init(string modName, List<IAchievement> achievements, List<ModEntry> mods, Transform? achievementList)
    {
        // Used to display mod name
        Name = transform.Find("Name").GetComponent<TMP_Text>();
        Name.text = modName.Truncate(20);
        Name.gameObject.SetActive(false);
        
        // Employs seamless merging with window
        Divider = transform.Find("Divide").gameObject;
        Divider.gameObject.SetActive(false);
        
        // Set mod image to one found with {name}.png
        // Current directory for now, CustomSounds has logic for all mods that have a folder named "Custom sounds" 
        
        var iconPath = $"./{modName}.png";
        var rawData = File.ReadAllBytes(iconPath);
        var iconTexture = new Texture2D(64, 64);
        iconTexture.LoadImage(rawData);
        
        // Create sprite with texture
        transform.Find("Icon").GetComponent<Image>().sprite = Sprite.Create(iconTexture,
            new Rect(0, 0, iconTexture.width, iconTexture.height), Vector2.zero);

        AllMods = mods;
        var achievementTemplate = achievementList.Find("Achievement").gameObject;
        achievementTemplate.SetActive(false);
        foreach (var achievement in achievements)
        {
            var achievementObj = Instantiate(achievementTemplate, achievementList);
            achievementObj.SetActive(true);
            var achievementEntry = achievementObj.AddComponent<AchievementEntry>();
            achievementEntry.Init(achievement);
            AchievementEntries.Add(achievementEntry);
        }
    }

    internal void Toggle(bool isActive)
    {
        if (isActive)
        {
            foreach (var mod in AllMods.Where(mod => mod != this)) mod.Toggle(false);
        }

        Name.gameObject.SetActive(isActive);
        Divider.gameObject.SetActive(isActive);
    }
}