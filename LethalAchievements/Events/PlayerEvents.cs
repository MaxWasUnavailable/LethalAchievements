using GameNetcodeStuff;
using HarmonyLib;
using LethalAchievements.Events.Patches;
using UnityEngine;

namespace LethalAchievements.Events;

internal static class PlayerEvents
{
    public delegate void PlayerEventHandler(PlayerControllerB player);
    public delegate void PlayerEventHandler<T>(PlayerControllerB player, in T context);
    
    public static event PlayerEventHandler? OnJumped;
    
    // these two are only triggered for the local player
    public static event PlayerEventHandler<EntranceType>? OnEnteredFacility;
    public static event PlayerEventHandler<EntranceType>? OnExitedFacility;
    
    public static event PlayerEventHandler<GrabbableObject>? OnPickedUpItem;
    
    public static event PlayerEventHandler<PlayerDiedContext>? OnDied;
    public static event PlayerEventHandler<PlayerDamagedContext>? OnDamaged;
    
    public static event PlayerEventHandler<EnemyAI>? OnDamagedEnemy;
    public static event PlayerEventHandler<EnemyAI>? OnKilledEnemy;

    [HarmonyPatch(typeof(PlayerControllerB))]
    internal static class PlayerPatches
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
        
        [HarmonyPrefix]
        [HarmonyPatch(nameof(PlayerControllerB.SwitchToItemSlot))]
        public static void SwitchToItemSlot_Postfix(PlayerControllerB __instance, GrabbableObject fillSlotWithItem)
        {
            if (fillSlotWithItem == null) 
                return;
            
            OnPickedUpItem?.Invoke(__instance, fillSlotWithItem);
        }
    }

    [HarmonyPatch(typeof(EntranceTeleport))]
    internal static class EntranceTeleportPatches 
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(EntranceTeleport.TeleportPlayer))]
        public static void TeleportPlayer_Postfix(EntranceTeleport __instance) 
        {
            var player = GameNetworkManager.Instance.localPlayerController;
            var type = __instance.name.Contains("Fire") ? EntranceType.Fire : EntranceType.Main;
            
            if (__instance.isEntranceToBuilding) {
                OnEnteredFacility?.Invoke(player, type);
            } else {
                OnExitedFacility?.Invoke(player, type);
            }
        } 
    }

    [HarmonyPatch(typeof(EnemyAI))]
    internal static class EnemyAIPatches
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(EnemyAI.HitEnemy))]
        public static void HitEnemy_Prefix(EnemyAI __instance, int force, PlayerControllerB playerWhoHit) {
            if (__instance.isEnemyDead || playerWhoHit == null)
                return;
            
            OnDamagedEnemy?.Invoke(playerWhoHit, __instance);
            
            if (__instance.enemyHP <= force) {
                OnKilledEnemy?.Invoke(playerWhoHit, __instance);
            }
        }
    }
}

internal enum EntranceType
{
    Main,
    Fire
}