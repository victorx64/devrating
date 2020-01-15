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

        public double WinnerNewRating(double current, IEnumerable<Match> matches)
        {
            foreach (var match in matches)
            {
                var loser = match.ContenderRating();

                for (var i = 0; i < match.Count(); ++i)
                {
                    var points = ExtraPointsOfA(current, loser);

                    current += points;
                    loser -= points;
                }
            }

            return current;
        }

        public double LoserNewRating(double current, Match match)
        {
            var winner = match.ContenderRating();

            for (var i = 0; i < match.Count(); ++i)
            {
                var points = ExtraPointsOfA(winner, current);

                winner += points;
                current -= points;
            }

            return current;
        }

        private double ExtraPointsOfA(double a, double b)
        {
            return _k * (1d - WinProbabilityOfA(a, b));
        }

        public double WinProbabilityOfA(double a, double b)
        {
            var qa = Math.Pow(10d, a / _n);

            var qb = Math.Pow(10d, b / _n);

            var ea = qa / (qa + qb);

            return ea;
        }
    }
}