using System;

namespace DevRating.Rating
{
    public class Elo : IPointsFormula
    {
        private readonly int _replacements;
        private readonly double _k;
        private readonly double _n;

        public Elo() : this(400, 1d, 400d)
        {
        }

        public Elo(int replacements, double k, double n)
        {
            _replacements = replacements;
            _k = k;
            _n = n;
        }

        public double UpdatedPoints(double outcome, IPlayer player, IPlayer opponent)
        {
            var expectedOutcome = ExpectedOutcome(player.Points(), opponent.Points());

            return opponent.Games() < _replacements
                ? player.Points()
                : player.Points() + _k * (outcome - expectedOutcome);
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