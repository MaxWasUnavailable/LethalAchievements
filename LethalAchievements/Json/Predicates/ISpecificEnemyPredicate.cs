namespace LethalAchievements.Config.Predicates;

public interface ISpecificEnemyPredicate<in T> : IPredicate<T> where T : EnemyAI {
    
}

public class 