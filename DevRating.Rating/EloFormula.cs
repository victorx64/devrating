using System;

namespace DevRating.Rating
{
    public sealed class EloFormula : Formula
    {
        private readonly double _k;
        private readonly double _n;
        private readonly double _default;

        public EloFormula() : this(2d, 400d, 1500d)
        {
        }

        public EloFormula(double k, double n, double @default)
        {
            _k = k;
            _n = n;
            _default = @default;
        }

        public double DefaultRating()
        {
            return _default;
        }

        public double WinnerNewRating(Match match)
        {
            return match.Winner() + WinnerExtraPoints(match.Winner(), match.Loser()) * match.Count();
        }

        public double LoserNewRating(Match match)
        {
            return match.Loser() - WinnerExtraPoints(match.Winner(), match.Loser()) * match.Count();
        }

        public double Reward(double rating, int count)
        {
            return WinProbability(rating, DefaultRating()) * count;
        }

        private double WinnerExtraPoints(double winner, double loser)
        {
            return _k * (1d - WinProbability(winner, loser));
        }

        private double WinProbability(double winner, double loser)
        {
            var qa = Math.Pow(10d, winner / _n);

            var qb = Math.Pow(10d, loser / _n);

            var ea = qa / (qa + qb);

            return ea;
        }
    }
}