using System;
using System.Collections.Generic;
using System.Linq;
using LethalAchievements.Config.Serialization;
using UnityEngine;

namespace LethalAchievements.Config;

internal static class ConditionHelper
{
    internal static bool Matches(bool? value, bool? predicate)
    {
        return predicate is null || value != null && predicate.Value == value;
    }

    internal static bool Matches<T>(T? value, Range<T>? range) where T : struct, IComparable<T>
    {
        return range is null || value != null && range.Contains(value.Value);
    }
    
    internal static bool Matches<T>(T? value, T[]? values)
    {
        return values is null || value != null && values.Contains(value);
    }

    internal static bool Matches<T>(T? value, IPredicate<T>? predicate)
    {
        return predicate is null || value != null && predicate.Check(value);
    }

    internal static bool Matches<T>(IReadOnlyList<T?>? values, IReadOnlyList<IPredicate<T>>? predicates)
    {
        if (predicates is null) return true;
        if (values is null) return false;
        
        Debug.Log($"Checking {values.Count} values against {predicates.Count} predicates.");

        var nonNullValues = values.Where(v => v != null).ToArray();
        
        if (nonNullValues.Length < predicates.Count) Debug.Log("Not enough values to assign all predicates."); 
        if (nonNullValues.Length < predicates.Count) return false; // not enough values to assign all predicates
        
        // this is boils down to an assignment problem
        
        var costs = new int[predicates.Count, nonNullValues.Length];
            
        for (var i = 0; i < predicates.Count; i++)
        {
            var matchedAny = false;
            
            for (var j = 0; j < nonNullValues.Length; j++)
            {
                var isMatch = predicates[i].Check(nonNullValues[j]!);
                costs[i, j] = isMatch ? 0 : 1; // the algorithm minimizes costs
                matchedAny |= isMatch;
            }
            
            if (!matchedAny) Debug.Log($"Predicate {i} didn't match any value; cannot be assigned to any value.");
            if (!matchedAny) return false; // predicate didn't match any value; cannot be assigned to any value
        }
        
        // assign predicates to values using the Hungarian algorithm
        var assignments = costs.FindAssignments();
        
        // check if any predicate is assigned to a value that it doesn't match
        // this means there was no valid assignment
        for (var i = 0; i < predicates.Count; i++)
        {
            if (costs[i, assignments[i]] != 0)
            {
                Debug.Log($"Predicate {i} is assigned to a value that it doesn't match.");
                return false;
            }
        }
        
        return true;
    }
    
    internal static bool All(params bool[] values)
    {
        return values.All(x => x);
    }
}