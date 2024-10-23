public static class SingletonManager
{
    public static BoardManager BoardManager => BoardManager.Instance;
    public static MatchChecker MatchChecker => MatchChecker.Instance;
    public static GameManager GameManager => GameManager.Instance;

}