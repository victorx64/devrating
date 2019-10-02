namespace DevRating.Rating
{
    public interface Formula
    {
        double NewPlayerRating();
        double WinnerNewRating(Match match);
        double LoserNewRating(Match match);
        double WinnerReward(Match match);
        double LoserReward(Match match);
        double BossRating();
    }
}