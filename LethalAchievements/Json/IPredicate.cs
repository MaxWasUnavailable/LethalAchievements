namespace LethalAchievements.Config;

public interface IPredicate<in T> {
    bool Check(T item);
}