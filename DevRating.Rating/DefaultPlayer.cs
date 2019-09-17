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

        public Player Winner(Player contender)
        {
            return UpdatedPlayer(1d, contender);
        }

        public Player Loser(Player contender)
        {
            return UpdatedPlayer(0d, contender);
        }
        
        private Player UpdatedPlayer(double outcome, Player contender)
        {
            var points = _formula.UpdatedPoints(outcome, _points, contender.Points());

            return new DefaultPlayer(_formula, points);
        }
    }
}