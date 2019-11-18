using System;
using System.Collections.Generic;
using System.Linq;
using DevRating.Domain;

namespace DevRating.EloRating
{
    public sealed class EloFormula : Formula
    {
        private readonly double _n;
        private readonly double _default;

        public EloFormula() : this(400d, 1500d)
        {
        }

        public EloFormula(double n, double @default)
        {
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
            var total = matches.Sum(MatchContenderRating);
            var games = matches.Sum(MatchCount);

            return (total + _n * games) / games;
        }

        public double LoserNewRating(double current, Match match)
        {
            var total = match.ContenderRating();
            var games = match.Count();

            return (total - _n * games) / games;
        }

        private long MatchCount(Match match)
        {
            return match.Count();
        }

        private double MatchContenderRating(Match match)
        {
            return match.ContenderRating();
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