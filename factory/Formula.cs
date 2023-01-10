namespace devrating.factory;

public interface Formula
{
    double DefaultRating();
    double WinnerRatingChange(double winnerOldRating, double loserOldRating);
    double SuggestedAdditionsPerWork(double rating);
    double WinProbabilityOfA(double a, double b);
}
