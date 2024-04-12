using System;
using System.Reflection;
using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace LethalAchievements.Events.Patches;

/// <summary>
///     Utility for tracking what enemy damages/kills a player.
/// </summary>
internal static class EnemyDamageSource
{
    /// <summary>
    ///     The last enemy which damaged a player this frame, or
    ///     null if no enemy has damaged the player this frame.
    /// </summary>
    public static EnemyAI? CurrentEnemy => Time.frameCount > _currentEnemyHitFrame ? null : _currentEnemy;

    private static EnemyAI? _currentEnemy;
    private static int _currentEnemyHitFrame = int.MinValue;

    private static readonly MethodInfo _enemyDamagePathMethod = AccessTools.Method(typeof(EnemyDamageSource), nameof(EnemyDamagePatch));

    private static readonly (Type, string)[] _enemyDamageMethods = {
        (typeof(SandSpiderAI), nameof(SandSpiderAI.OnCollideWithPlayer)),
        (typeof(BlobAI), nameof(BlobAI.OnCollideWithPlayer)),
        (typeof(JesterAI), nameof(JesterAI.killPlayerAnimation)),
        (typeof(DressGirlAI), nameof(DressGirlAI.OnCollideWithPlayer)),
        (typeof(NutcrackerEnemyAI), nameof(NutcrackerEnemyAI.LegKickPlayer)),
        (typeof(RedLocustBees), nameof(RedLocustBees.BeeKillPlayerOnLocalClient)),
        (typeof(SandWormAI), nameof(SandWormAI.EatPlayer)),
        (typeof(CentipedeAI), nameof(CentipedeAI.DamagePlayerOnIntervals)),
        (typeof(FlowermanAI), nameof(FlowermanAI.killAnimation))
    };

    private static void EnemyDamagePatch(EnemyAI __instance)
    {
        _currentEnemy = __instance;
        _currentEnemyHitFrame = Time.frameCount;
    }

    internal static void Patch(Harmony harmony)
    {
        var prefix = new HarmonyMethod(_enemyDamagePathMethod);
        
        foreach (var (type, methodName) in _enemyDamageMethods)
        {
            harmony.Patch(AccessTools.Method(type, methodName), prefix: prefix);
        }
    }
}