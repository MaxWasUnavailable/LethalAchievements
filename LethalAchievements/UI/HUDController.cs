using System.Collections.Generic;
using System.Text.RegularExpressions;
using LethalAchievements.Features;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LethalAchievements.UI;

internal class HUDController : MonoBehaviour
{
    private static Button? _openButton;
    private static Button? _exitButton;
    private static Transform? _achievementContainer;
    private static Transform? _achievementTemplate;
    private static Transform? _modContainer;
    private static Transform? _modTemplate;
    private TMP_Text? _exitButtonText;
    private TMP_Text? _openButtonText;
    private Transform? _ui;

    internal static AudioSource? ClickSfx { get; private set; }
    internal static List<ModEntry> ModList { get; } = [];
    internal static HUDController? Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        Instance.hideFlags = HideFlags.HideAndDontSave;
        DontDestroyOnLoad(Instance.gameObject);

        // Get all OpenButton components
        _openButton = transform.Find("OpenButton").GetComponent<Button>();
        _openButtonText = transform.Find("OpenButton/Text").GetComponent<TMP_Text>();

        // Get all ExitButton components
        _exitButton = transform.Find("ExitButton").GetComponent<Button>();
        _exitButtonText = transform.Find("ExitButton/Text").GetComponent<TMP_Text>();

        // Get UI components
        _ui = transform.Find("UI");
        _achievementContainer = transform.Find("UI/AchievementListContainer/Scroll View/Viewport/Content");
        _achievementTemplate = _achievementContainer.Find("Achievement");

        _modContainer = transform.Find("UI/ModTabContainer/Scroll View/Viewport/Content");
        _modTemplate = _modContainer.Find("Mod");

        ClickSfx = _modContainer.Find("ClickSFX").GetComponent<AudioSource>();

        // Add method calls on button clicks
        _openButton.onClick.AddListener(OpenUI);
        _exitButton.onClick.AddListener(CloseUI);

        // Set initial pane to only display the achievements button
        _ui.gameObject.SetActive(false);
        _modTemplate.gameObject.SetActive(false);
        _achievementTemplate.gameObject.SetActive(false);
        _exitButton.gameObject.SetActive(false);
        _openButton.gameObject.SetActive(true);

        LethalAchievements.Logger?.LogInfo("UI loaded!");
    }

    private void Update()
    {
        if (_openButton!.gameObject.activeSelf)
            _openButtonText!.color = _openButton.isPointerInside
                ? Color.black
                : new Color32(255, 126, 63, 150);
        if (_exitButton!.gameObject.activeSelf)
            _exitButtonText!.color = _exitButton.isPointerInside
                ? Color.black
                : new Color32(255, 50, 0, 255);
    }

    private void OpenUI()
    {
        _openButton!.gameObject.SetActive(false);
        _ui!.gameObject.SetActive(true);
        _exitButton!.gameObject.SetActive(true);
    }

    private void CloseUI()
    {
        _exitButton!.gameObject.SetActive(false);
        _ui!.gameObject.SetActive(false);
        _openButton!.gameObject.SetActive(true);
    }

    internal static void InitializeUI()
    {
        // Logic to add mods that have achievements
        foreach (var mod in AchievementManager.AchievementRegistry.GetAchievementsByPlugins())
        {
            // Get RegEx'd name to display on tab
            var regEx = Regex.Match(mod.Key.Metadata.Name, @"^([^.]*)$|^(.*?)\.(.*)$");
            var modName = regEx.Groups.Count < 2 ? regEx.Groups[1].Value : regEx.Groups[3].Value;

            LethalAchievements.Logger?.LogInfo($"Adding {mod.Key.Metadata.Name} to the mod tab");

            // Create tab for given mod
            var modObj = Instantiate(_modTemplate, _modContainer);
            var modEntry = modObj!.gameObject.AddComponent<ModEntry>();
            modEntry.Init(modName, mod.Value, ModList, _achievementContainer!);
            modObj.gameObject.SetActive(true);

            ModList.Add(modEntry);
        }

        // Disable game object
        Instance!.gameObject.SetActive(false);
    }
}