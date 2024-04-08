using GameNetcodeStuff;

namespace LethalAchievements.Config;

public struct Context
{
    /// <summary>
    ///    The achievement being checked.
    /// </summary>
    public ConfigAchievement Achievement;
    
    /// <summary>
    ///     The player being checked.
    /// </summary>
    public PlayerControllerB? Player;
    
    /// <summary>
    ///     Creates a copy of this context.
    /// </summary>
    public Context Copy()
    {
        return new Context {
            Player = Player
        };
    }
 
    /// <summary>
    ///     Creates a context with default values.
    ///     <list type="bullet">
    ///         <item>
    ///             <description><see cref="Player"/> is set to the local player.</description>
    ///         </item>
    ///     </list>
    /// </summary>
    /// <returns></returns>
    public static Context Default()
    {
        return new Context {
            Player = StartOfRound.Instance.localPlayerController
        };
    }
}