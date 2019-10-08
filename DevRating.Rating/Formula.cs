namespace DevRating.Rating
{
    public interface Formula
    {
        double DefaultRating();
        double WinnerNewRating(Match match);
        double LoserNewRating(Match match);
        double Reward(double rating, int count);
    }
}