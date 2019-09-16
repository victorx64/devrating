namespace DevRating.Rating
{
    public sealed class DefaultPlayer : Player
    {
        private readonly PointsFormula _formula;
        private readonly double _points;

        public DefaultPlayer() : this(new EloPointsFormula(), 1200)
        {
        }

        public DefaultPlayer(PointsFormula formula) : this(formula, 1200)
        {
        }

        public DefaultPlayer(PointsFormula formula, double points)
        {
            _formula = formula;
            _points = points;
        }

        public double Points()
        {
            return _points;
        }

        public Player Winner(Player opponent)
        {
            return UpdatedPlayer(1d, opponent);
        }

        public Player Loser(Player opponent)
        {
            return UpdatedPlayer(0d, opponent);
        }
        
        private Player UpdatedPlayer(double outcome, Player opponent)
        {
            var points = _formula.UpdatedPoints(0d, _points, opponent.Points());

            return new DefaultPlayer(_formula, points);
        }
    }
}