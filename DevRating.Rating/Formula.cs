namespace DevRating.Rating
{
    public interface Formula
    {
        double WinProbability(double winner, double loser);
        double WinnerExtraPoints(double winner, double loser);
    }
}