using System;
using System.Collections.Generic;
using System.Linq;
using LethalAchievements.Config.Serialization;

namespace LethalAchievements.Config;

internal static class ConditionHelper
{
    internal static bool Matches(bool value, bool? predicate)
    {
        return predicate is null || predicate.Value == value;
    }

    internal static bool Matches<T>(T value, Range<T>? range) where T : struct, IComparable<T>
    {
        return range is null || range.Contains(value);
    }
    
    internal static bool Contains<T>(T value, T[]? values)
    {
        return values is null || values.Contains(value);
    }

    internal static bool Predicate<T>(T? value, IPredicate<T>? predicate)
    {
        return predicate is null || value != null && predicate.Check(value);
    }
    
    internal static bool Predicate<T>(IEnumerable<T?>? values, IEnumerable<IPredicate<T>>? predicates)
    {
        return 
            predicates is null ||
            values != null &&
            predicates.All(pred => values.Any(value => value != null && pred.Check(value)));
    }
    
    internal static bool All(params bool[] values)
    {
        return values.All(x => x);
    }
}