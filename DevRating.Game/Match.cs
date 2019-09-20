namespace DevRating.Game
{
    public interface Match
    {
        string Player();
        double Points();
        string Commit();
        string Contender();
        int Rounds();
        double Reward();
    }
}