using System;

namespace DevRating.Rating
{
    public sealed class DefaultPlayer : Player
    {
        private readonly PointsFormula _formula;
        private readonly double _points;
        private readonly int _wins;
        private readonly int _defeats;

        public DefaultPlayer(PointsFormula formula) : this(formula, 1200, 0, 0)
        {
        }

        public DefaultPlayer(PointsFormula formula, double points, int wins, int defeats)
        {
            _formula = formula;
            _points = points;
            _wins = wins;
            _defeats = defeats;
        }

        public double Points()
        {
            return _points;
        }

        public int Games()
        {
            return _wins + _defeats;
        }

        public Player Winner(Player opponent)
        {
            var points = _formula.UpdatedPoints(1d, _points, opponent.Points());

            return new DefaultPlayer(_formula, points, _wins + 1, _defeats);
        }

        public Player Loser(Player opponent)
        {
            var points = _formula.UpdatedPoints(0d, _points, opponent.Points());

            return new DefaultPlayer(_formula, points, _wins, _defeats + 1);
        }

        public void Print(Output output)
        {
            output.WriteLine(FormattableString.Invariant($"{_wins}, {_defeats}, {_points:F2}"));
        }

        public int CompareTo(Player other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (ReferenceEquals(null, other))
            {
                return 1;
            }
            
            return _points.CompareTo(other.Points());
        }
    }
}