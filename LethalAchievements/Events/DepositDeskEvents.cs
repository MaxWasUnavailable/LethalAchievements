using GameNetcodeStuff;
using HarmonyLib;

namespace LethalAchievements.Events;

internal class DepositDeskEvents
{
    public delegate void DepositDeskEventHandler(DepositItemsDesk desk);
    public delegate void DepositDeskEventHandler<T>(DepositItemsDesk desk, in T context);
    
    public static event DepositDeskEventHandler<ItemDepositedContext>? OnItemDeposited;
    public static event DepositDeskEventHandler<ItemsSoldContext>? OnItemsSold;

    internal static class Patches
    {
        [HarmonyPatch(typeof(DepositItemsDesk))]
        internal static class DepositItemsDeskPatches
        {
            [HarmonyPrefix]
            [HarmonyPatch(nameof(DepositItemsDesk.PlaceItemOnCounter))]
            public static void PlaceItemOnCounter_Prefix(DepositItemsDesk __instance, PlayerControllerB playerWhoTriggered)
            {
                if (__instance.deskObjectsContainer.GetComponentsInChildren<GrabbableObject>().Length >= 12 || 
                    __instance.inGrabbingObjectsAnimation || 
                    !(GameNetworkManager.Instance != null) || 
                    !(playerWhoTriggered == GameNetworkManager.Instance.localPlayerController)
                )
                    return;
                
                OnItemDeposited?.Invoke(__instance, new ItemDepositedContext(
                    playerWhoTriggered,
                    playerWhoTriggered.currentlyHeldObjectServer
                ));
            }
            
            [HarmonyPostfix]
            [HarmonyPatch(nameof(DepositItemsDesk.SellItemsClientRpc))]
            public static void SellItemsClientRpc_Postfix(DepositItemsDesk __instance, int itemProfit)
            {
                OnItemsSold?.Invoke(__instance, new ItemsSoldContext(
                    __instance.deskObjectsContainer.GetComponentsInChildren<GrabbableObject>(),
                    itemProfit
                ));
            }
        }
    } 
}