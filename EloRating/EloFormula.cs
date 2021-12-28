// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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

        public EloFormula() : this(1d, 400d, 1500d)
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
            double WinnerExtraPointsForMatch(Match m)
            {
                return WinnerExtraPoints(current, m.ContenderRating(), m.Count());
            }

            return current + matches.Sum(WinnerExtraPointsForMatch);
        }

        public double LoserNewRating(double current, Match match)
        {
            return current - WinnerExtraPoints(match.ContenderRating(), current, match.Count());
        }

        private double WinnerExtraPoints(double winner, double loser, uint count)
        {
            var summary = 0d;

            for (var i = 0; i < count; ++i)
            {
                var extra = WinnerExtraPoints(winner, loser);

                summary += extra;
                winner += extra;
                loser -= extra;
            }

            return summary;
        }

        private double WinnerExtraPoints(double winner, double loser)
        {
            return _k * (1d - WinProbabilityOfA(winner, loser));
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