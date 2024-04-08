using System.Linq;

namespace LethalAchievements.Config;

internal static class Condition
{
    internal static bool Matches(bool? predicate, bool value)
    {
        if (predicate is null)
            return true;
        
        return predicate.Value == value;
    }
    
    internal static bool All(params bool[] values)
    {
        return values.All(x => x);
    }
}