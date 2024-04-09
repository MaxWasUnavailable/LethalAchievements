using GameNetcodeStuff;
using HarmonyLib;
using LethalAchievements.Events.Patches;
using UnityEngine;

namespace LethalAchievements.Events;

/// <summary>
///     Provides callbacks for player events.
/// </summary>
public static class PlayerEvents
{
    /// <summary>
    ///     A method that handles player events with no additional context.
    /// </summary>
    /// <param name="player">The player that triggered the event.</param>
    public delegate void PlayerEventHandler(PlayerControllerB player);
    
    /// <summary>
    ///     A method that handles player events with additional context.
    /// </summary>
    /// <param name="player">The player that triggered the event.</param>
    /// <typeparam name="T">The type of the context.</typeparam>
    public delegate void PlayerEventHandler<T>(PlayerControllerB player, in T context);

    /// <summary>
    ///     Invoked when a player jumps.
    /// </summary>
    /// <remarks>
    ///     Note that this is invoked for <b>every player, not just the local one!</b>
    ///     If you want to only listen for the local player, you can check <see cref="PlayerControllerB.IsOwner"/>.
    /// </remarks>
    public static event PlayerEventHandler? OnJumped;

    /// <summary>
    ///     Invoked when a player dies.
    /// </summary>
    /// <remarks>
    ///     Note that this is invoked for <b>every player, not just the local one!</b>
    ///     If you want to only listen for the local player, you can check <see cref="PlayerControllerB.IsOwner"/>.
    /// </remarks>
    public static event PlayerEventHandler<PlayerDiedContext>? OnDied;

    /// <summary>
    ///     Invoked when a player is damaged.
    /// </summary>
    /// <remarks>
    ///     Note that this is invoked for <b>every player, not just the local one!</b>
    ///     If you want to only listen for the local player, you can check <see cref="PlayerControllerB.IsOwner"/>.
    /// </remarks>
    public static event PlayerEventHandler<PlayerDamagedContext>? OnDamaged;

    [HarmonyPatch(typeof(PlayerControllerB))]
    internal static class Patches
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(PlayerControllerB.PlayerJump))]
        public static void PlayerJump_Prefix(PlayerControllerB __instance)
        {
            OnJumped?.Invoke(__instance);
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(PlayerControllerB.KillPlayer))]
        public static void KillPlayer_Prefix(PlayerControllerB __instance, Vector3 bodyVelocity, CauseOfDeath causeOfDeath)
        {
            if (!__instance.IsOwner || __instance.isPlayerDead || !__instance.AllowPlayerDeath())
                return;

            var context = new PlayerDiedContext(bodyVelocity, causeOfDeath, EnemyDamageSource.CurrentEnemy);
            OnDied?.Invoke(__instance, context);
        }

        [HarmonyPrefix]
        [HarmonyPatch(nameof(PlayerControllerB.DamagePlayer))]
        public static void DamagePlayer_Prefix(PlayerControllerB __instance, int damageNumber, CauseOfDeath causeOfDeath)
        {
            if (!__instance.IsOwner || __instance.isPlayerDead || !__instance.AllowPlayerDeath())
                return;

            var context = new PlayerDamagedContext(damageNumber, causeOfDeath, EnemyDamageSource.CurrentEnemy);
            OnDamaged?.Invoke(__instance, context);
        }
    }
}

/// <summary>
///     Context for the <see cref="PlayerEvents.OnDied"/> event.
/// </summary>
public struct PlayerDiedContext
{
    /// <summary>
    ///     The velocity applied to the player's body.
    /// </summary>
    public Vector3 BodyVelocity;

    /// <summary>
    ///     The cause of death.
    /// </summary>
    public CauseOfDeath CauseOfDeath;

    /// <summary>
    ///     The enemy that killed the player, if any.
    /// </summary>
    public EnemyAI? KillerEnemy;

    public PlayerDiedContext(Vector3 bodyVelocity, CauseOfDeath causeOfDeath, EnemyAI? killerEnemy)
    {
        BodyVelocity = bodyVelocity;
        CauseOfDeath = causeOfDeath;
        KillerEnemy = killerEnemy;
    }
}

/// <summary>
///     Context for the <see cref="PlayerEvents.OnDamaged"/> event.
/// </summary>
public struct PlayerDamagedContext
{
    /// <summary>
    ///    The amount of damage dealt to the player.
    /// </summary>
    public int Amount;

    /// <summary>
    ///     The cause of death if the player dies.
    /// </summary>
    public CauseOfDeath CauseOfDeath;

    /// <summary>
    ///    The enemy that damaged the player, if any.
    /// </summary>
    public EnemyAI? AttackerEnemy;

    public PlayerDamagedContext(int amount, CauseOfDeath causeOfDeath, EnemyAI? attackerEnemy)
    {
        Amount = amount;
        CauseOfDeath = causeOfDeath;
        AttackerEnemy = attackerEnemy;
    }
}