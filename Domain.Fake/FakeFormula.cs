using System.Collections.Generic;

namespace DevRating.Domain.Fake
{
    public sealed class FakeFormula : Formula
    {
        public double DefaultRating()
        {
            throw new System.NotImplementedException();
        }

        public double WinnerNewRating(double current, IEnumerable<Match> matches)
        {
            throw new System.NotImplementedException();
        }

        public double LoserNewRating(double current, Match match)
        {
            throw new System.NotImplementedException();
        }

        public double WinProbabilityOfA(double a, double b)
        {
            throw new System.NotImplementedException();
        }
    }
}