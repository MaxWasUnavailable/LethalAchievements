using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace LethalAchievements.Events.Patches;

/// <summary>
///     Utility for tracking what enemy damages/kills a player.
/// </summary>
internal static class EnemyDamageSource
{
    private static EnemyAI? _currentEnemy;
    private static int _currentEnemyHitFrame = int.MinValue;

    private static readonly MethodInfo EnemyDamagePatchMethod =
        AccessTools.Method(typeof(EnemyDamageSource), nameof(EnemyDamagePatch));

    private static readonly (Type, string)[] EnemyDamageMethods =
    [
        (typeof(SandSpiderAI), nameof(SandSpiderAI.OnCollideWithPlayer)),
        (typeof(BlobAI), nameof(BlobAI.OnCollideWithPlayer)),
        (typeof(JesterAI), nameof(JesterAI.killPlayerAnimation)),
        (typeof(DressGirlAI), nameof(DressGirlAI.OnCollideWithPlayer)),
        (typeof(NutcrackerEnemyAI), nameof(NutcrackerEnemyAI.LegKickPlayer)),
        (typeof(RedLocustBees), nameof(RedLocustBees.BeeKillPlayerOnLocalClient)),
        (typeof(SandWormAI), nameof(SandWormAI.EatPlayer)),
        (typeof(CentipedeAI), nameof(CentipedeAI.DamagePlayerOnIntervals)),
        (typeof(FlowermanAI), nameof(FlowermanAI.killAnimation))
    ];

    /// <summary>
    ///     The last enemy which damaged a player this frame.
    /// </summary>
    public static EnemyAI? CurrentEnemy => Time.frameCount > _currentEnemyHitFrame ? null : _currentEnemy;

    private static void EnemyDamagePatch(EnemyAI __instance)
    {
        _currentEnemy = __instance;
        _currentEnemyHitFrame = Time.frameCount;
    }

    private static void PatchEnemyDamageMethod(Type type, string methodName, Harmony harmony)
    {
        harmony.Patch(
            AccessTools.Method(type, methodName),
            new HarmonyMethod(EnemyDamagePatchMethod)
        );
    }

    internal static void Patch(Harmony harmony)
    {
        foreach (var (type, methodName) in EnemyDamageMethods) PatchEnemyDamageMethod(type, methodName, harmony);
    }
}