using System;

namespace DevRating.Rating
{
    public sealed class Elo : PointsFormula
    {
        private readonly double _k;
        private readonly double _n;

        public Elo() : this(1d, 400d)
        {
        }

        public Elo(double k, double n)
        {
            _k = k;
            _n = n;
        }

        public double UpdatedPoints(double outcome, double points, double contender)
        {
            var expectedOutcome = ExpectedOutcome(points, contender);

            return points + _k * (outcome - expectedOutcome);
        }

        private double ExpectedOutcome(double points, double opponentPoints)
        {
            var ra = points;

            var rb = opponentPoints;

            var qa = Math.Pow(10d, ra / _n);

            var qb = Math.Pow(10d, rb / _n);

            var ea = qa / (qa + qb);

            return ea;
        }
    }
}