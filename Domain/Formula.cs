using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface Formula
    {
        double DefaultRating();
        double WinnerNewRating(double current, IEnumerable<Match> matches);
        double LoserNewRating(double current, Match match);
        double WinProbabilityOfA(double a, double b);
    }
}