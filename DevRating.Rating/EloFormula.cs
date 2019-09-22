using System;

namespace DevRating.Rating
{
    public sealed class EloFormula : Formula
    {
        private readonly double _k;
        private readonly double _n;

        public EloFormula() : this(2d, 400d)
        {
        }

        public EloFormula(double k, double n)
        {
            _k = k;
            _n = n;
        }

        public double WinnerExtraPoints(double winner, double loser)
        {
            var probability = WinProbability(winner, loser);

            return _k * (1d - probability);
        }

        public double WinProbability(double winner, double loser)
        {
            var qa = Math.Pow(10d, winner / _n);

            var qb = Math.Pow(10d, loser / _n);

            var ea = qa / (qa + qb);

            return ea;
        }
    }
}