namespace DevRating.Rating
{
    public interface Formula
    {
        double DefaultRating();
        double Reward(double rating, int count);
        double WinnerNewRating(double winner, double loser, int count);
        double LoserNewRating(double winner, double loser, int count);
    }
}