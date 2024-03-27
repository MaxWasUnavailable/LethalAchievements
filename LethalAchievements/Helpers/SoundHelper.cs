namespace LethalAchievements.Helpers;

/// <summary>
///     Helper class for sound-related features.
/// </summary>
public static class SoundHelper
{
    /// <summary>
    ///     Plays the level up sound.
    /// </summary>
    public static void PlayLevelUpSound()
    {
        HUDManager.Instance.UIAudio.PlayOneShot(HUDManager.Instance.levelIncreaseSFX);
    }
}