using System;
using HarmonyLib;

namespace LethalAchievements.Events;

internal static class QuotaEvents {
    public static event Action? OnNewQuota;
    
    [HarmonyPatch(typeof(TimeOfDay))]
    public static class TimeOfDayPatches {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(TimeOfDay.SyncNewProfitQuotaClientRpc))]
        public static void SyncNewProfitQuotaClientRpcPrefix() {
            OnNewQuota?.Invoke();
        }
    }
}