namespace DevRating.Domain
{
    public interface Formula
    {
        double DefaultRating();
        double Reward(double rating, uint count);
        double WinnerNewRating(double winner, double loser, uint count);
        double LoserNewRating(double winner, double loser, uint count);
    }
}