using GameNetcodeStuff;

namespace LethalAchievements.Helpers;

/// <summary>
///     Helper class for player-related features.
/// </summary>
public static class PlayerHelper
{
    /// <summary>
    ///     Gets the name of the specified player.
    /// </summary>
    /// <param name="player"> The player to get the name of. </param>
    /// <returns> The name of the specified player. </returns>
    public static string GetPlayerName(PlayerControllerB player)
    {
        return player.playerUsername;
    }

    /// <summary>
    ///     Gets the name of the current player.
    /// </summary>
    /// <returns> The name of the current player. </returns>
    public static string GetCurrentPlayerName()
    {
        return GetPlayerName(GameNetworkManager.Instance.localPlayerController);
    }
}