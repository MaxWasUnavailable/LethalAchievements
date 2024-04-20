namespace LethalAchievements.Json;

/// <summary>
///     A predicate.
/// </summary>
public interface IPredicate<in T>
{
    /// <summary>
    ///     Checks if the given item satisfies the predicate.
    /// </summary>
    bool Check(T item);
}