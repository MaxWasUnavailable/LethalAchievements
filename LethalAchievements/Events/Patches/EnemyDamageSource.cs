﻿using System;
using System.Reflection;
using GameNetcodeStuff;
using HarmonyLib;
using UnityEngine;

namespace LethalAchievements.Events.Patches;

public static class EnemyDamageSource 
{
    /// <summary>
    ///     The last enemy which damaged a player this frame.
    /// </summary>
    public static EnemyAI? CurrentEnemy => Time.frameCount > _currentEnemyHitFrame ? null : _currentEnemy;
    
    static EnemyAI? _currentEnemy;
    static int _currentEnemyHitFrame = int.MinValue;

    static readonly MethodInfo _enemyDamagePathMethod = AccessTools.Method(typeof(EnemyDamageSource), nameof(EnemyDamagePatch));

    static readonly (Type, string)[] _enemyDamageMethods = {
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

    static void EnemyDamagePatch(EnemyAI __instance)
    {
        _currentEnemy = __instance;
        _currentEnemyHitFrame = Time.frameCount;
    }
        
    static void PatchEnemyDamageMethod(Type type, string methodName, Harmony harmony)
    {
        harmony.Patch(
            AccessTools.Method(type, methodName), 
            prefix: new HarmonyMethod(_enemyDamagePathMethod)
        );
    }

    internal static void Patch(Harmony harmony)
    {
        foreach (var (type, methodName) in _enemyDamageMethods) {
            PatchEnemyDamageMethod(type, methodName, harmony);
        }
    }
}