using System;

namespace LethalAchievements.Config;

/// <summary>
///    A trigger for <see cref="JsonAchievement"/>. Any class implementing this interface
///    is automatically available in the JSON schema, by specific its type name.
/// </summary>
public interface ITrigger
{
    /// <summary>
    ///    Alert the owner <see cref="JsonAchievement"/> that this trigger has been triggered.
    ///    The achievement will check the conditions for this trigger, and if all conditions are met,
    ///    the achievement is completed 🥳.
    /// </summary>
    event Action<Context> OnTriggered;
    
    /// <summary>
    ///    Initializes the trigger, called automatically by the owner Achievement.
    ///    Should hook up any event listeners, etc...
    /// </summary>
    void Initialize();
    
    /// <summary>
    ///    Uninitializes the trigger, called automatically by the owner Achievement.
    ///    
    /// </summary>
    void Uninitialize();
}