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
    
    public static event PlayerEventHandler<GrabbableObject>? OnPickedUpItem;
    
    public static event PlayerEventHandler<PlayerDiedContext>? OnDied;
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
        
        [HarmonyPrefix]
        [HarmonyPatch(nameof(PlayerControllerB.SwitchToItemSlot))]
        public static void SwitchToItemSlot_Postfix(PlayerControllerB __instance, GrabbableObject fillSlotWithItem)
        {
            if (fillSlotWithItem == null) 
                return;
            
            OnPickedUpItem?.Invoke(__instance, fillSlotWithItem);
        }
    }
}