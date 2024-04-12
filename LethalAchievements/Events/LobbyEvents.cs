using System;
using HarmonyLib;

namespace LethalAchievements.Events;

internal static class LobbyEvents
{
    public static event Action? OnEnteredOrbit;

    [HarmonyPatch(typeof(StartOfRound))]
    internal static class StartOfRoundPatches
    {
        [HarmonyPostfix]
        [HarmonyPatch(nameof(StartOfRound.EndOfGameClientRpc))]
        public static void EndOfGameClientRpc_Postfix()
        {
            OnEnteredOrbit?.Invoke();
        }
    }
}