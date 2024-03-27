using UnityEngine;

namespace LethalAchievements.Helpers;

/// <summary>
///     Helper class for effect-related features.
/// </summary>
public static class EffectHelper
{
    private static readonly int Shake = Animator.StringToHash("Shake");

    /// <summary>
    ///     Play a shake effect.
    /// </summary>
    public static void PlayShakeEffect()
    {
        HUDManager.Instance.playerLevelBoxAnimator.SetTrigger(Shake);
    }
}