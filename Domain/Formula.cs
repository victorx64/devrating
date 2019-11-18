using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface Formula
    {
        double DefaultRating();
        double Reward(double rating, uint count);
        double WinnerNewRating(double current, IEnumerable<Match> matches);
        double LoserNewRating(double current, Match match);
    }
}