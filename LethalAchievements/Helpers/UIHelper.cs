namespace LethalAchievements.Helpers;

/// <summary>
///     Helper class for UI-related features.
/// </summary>
public static class UIHelper
{
    /// <summary>
    ///     Displays a status message on the HUD.
    /// </summary>
    /// <param name="message"> The message to display. </param>
    public static void DisplayStatusMessage(string message)
    {
        HUDManager.Instance.DisplayStatusEffect(message);
    }

    /// <summary>
    ///     Displays a tip on the HUD.
    /// </summary>
    /// <param name="title"> The title of the tip. </param>
    /// <param name="message"> The message of the tip. </param>
    /// <param name="warningStyle"> Whether the tip should be displayed in a warning style. </param>
    public static void DisplayTip(string title, string message, bool warningStyle = false)
    {
        HUDManager.Instance.DisplayTip(title, message, warningStyle);
    }
    
    /// <summary>
    ///     Displays a global notification on the HUD.
    /// </summary>
    /// <param name="message"> The message to display. </param>
    public static void DisplayGlobalNotification(string message)
    {
        HUDManager.Instance.DisplayGlobalNotification(message);
    }

    /// <summary>
    ///     Sends a chat message to all players on behalf of the server.
    /// </summary>
    /// <param name="message"> The message to send. </param>
    public static void SendServerChatMessage(string message)
    {
        HUDManager.Instance.AddTextToChatOnServer(message);
    }
}