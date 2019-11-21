using System;
using System.Collections.Generic;
using System.Linq;
using DevRating.Domain;

namespace DevRating.EloRating
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

        public double Reward(double rating, uint count)
        {
            return WinProbability(rating, DefaultRating()) * count;
        }

        public double WinnerNewRating(double current, IEnumerable<Match> matches)
        {
            double MatchExtraPoints(Match match)
            {
                return WinnerExtraPoints(current, match.ContenderRating()) * match.Count();
            }

            return current + matches.Sum(MatchExtraPoints);
        }

        public double LoserNewRating(double current, Match match)
        {
            return current - WinnerExtraPoints(current, match.ContenderRating()) * match.Count();
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