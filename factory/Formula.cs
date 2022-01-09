namespace devrating.factory;

public interface Formula
{
    double DefaultRating();
    double WinnerRatingChange(double winnerOldRating, double loserOldRating);
}
