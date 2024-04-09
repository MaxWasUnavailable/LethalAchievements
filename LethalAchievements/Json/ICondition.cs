namespace LethalAchievements.Config;

public interface ICondition
{
    bool Evaluate(in Context context);
}