namespace DevRating.Game
{
    public interface Game
    {
        double PointsAfter();
        string Commit();
        string Contender();
        int Rounds();
        double Reward();
    }
}