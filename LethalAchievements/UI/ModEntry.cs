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
    private List<ModEntry> _allMods = [];
    private GameObject? _divider;
    private bool _isExpanded;
    private TMP_Text? _name;
    internal Dictionary<IAchievement, AchievementEntry> AchievementEntries { get; } = new();

    internal void Init(string modName, List<IAchievement> achievements, List<ModEntry> mods, Transform achievementList)
    {
        InitName(modName);
        InitDivider();
        InitModIcon(modName);
        _allMods = mods;

        // Add listener to button
        transform.GetComponent<Button>().onClick.AddListener(OnClick);

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

    private void InitName(string modName)
    {
        _name = transform.Find("Name").GetComponent<TMP_Text>();
        _name.text = modName.Truncate(30);
        _name.gameObject.SetActive(false);
    }

    private void InitDivider()
    {
        _divider = transform.Find("Divide").gameObject;
        _divider.gameObject.SetActive(false);
    }

    private void InitModIcon(string modName)
    {
        // TODO: This system needs a rework

        // From Nebby:
        // Set mod image to one found with {name}.png
        // Make a class for different types of icon loadings etc etc
        // Current directory for now, CustomSounds has logic for all mods that have a folder named "Custom sounds" 
        var iconPath = $"./{modName}.png";

        if (!File.Exists(iconPath))
        {
            // Check if the mod has a folder with the same name
            iconPath = $"./{modName}/{modName}.png";
            if (!File.Exists(iconPath))
            {
                LethalAchievements.Logger?.LogDebug($"No icon found for {modName}! - Using default icon.");
                return;
            }
        }

        try
        {
            var rawData = File.ReadAllBytes(iconPath);
            var iconTexture = new Texture2D(64, 64);
            iconTexture.LoadImage(rawData);

            // Create sprite with texture
            transform.Find("Icon").GetComponent<Image>().sprite = Sprite.Create(iconTexture,
                new Rect(0, 0, iconTexture.width, iconTexture.height), Vector2.zero);
        }
        catch
        {
            LethalAchievements.Logger?.LogError($"Failed to load icon for {modName}!");
        }
    }

    private void OnClick()
    {
        HUDController.ClickSfx?.Play();
        // Sets any mods not clicked to not be displayed
        if (!_isExpanded)
            Toggle(!_isExpanded);
    }

    private void Toggle(bool isActive)
    {
        if (isActive)
            foreach (var mod in _allMods.Where(mod => mod != this))
                mod.Toggle(false);

        foreach (var achievement in AchievementEntries)
            achievement.Value.gameObject.SetActive(isActive);

        _isExpanded = isActive;
        _name?.gameObject.SetActive(isActive);
        _divider?.gameObject.SetActive(isActive);
    }
}