using System.Collections.Generic;
using System.Linq;

namespace DevRating.Domain.Fake
{
    public sealed class FakeFormula : Formula
    {
        private readonly double _default;
        private readonly double _increase;

        public FakeFormula() : this(10d, 1d)
        {
        }

        public FakeFormula(double @default, double increase)
        {
            _default = @default;
            _increase = increase;
        }

        public double DefaultRating()
        {
            return _default;
        }

        public double WinnerNewRating(double current, IEnumerable<Match> matches)
        {
            double MatchExtraPoints(Match match)
            {
                return _increase * match.Count();
            }

            return current + matches.Sum(MatchExtraPoints);
        }

        public double LoserNewRating(double current, Match match)
        {
            return current - _increase * match.Count();
        }

        public double WinProbabilityOfA(double a, double b)
        {
            throw new System.NotImplementedException();
        }
    }
}