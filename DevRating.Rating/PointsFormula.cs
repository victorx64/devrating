namespace DevRating.Rating
{
    public interface PointsFormula
    {
        double WinProbability(double winner, double loser);
        double WinnerExtraPoints(double winner, double loser);
    }
}