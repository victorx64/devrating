namespace DevRating.Rating
{
    public sealed class Player : IPlayer
    {
        private readonly IPointsFormula _formula;
        private readonly double _points;
        private readonly int _wins;
        private readonly int _defeats;

        public Player(IPointsFormula formula) : this(formula, 1200, 0, 0)
        {
        }

        public Player(IPointsFormula formula, double points, int wins, int defeats)
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

        public IPlayer Winner(IPlayer opponent)
        {
            var points = _formula.UpdatedPoints(1d, this, opponent);

            return new Player(_formula, points, _wins + 1, _defeats);
        }

        public IPlayer Loser(IPlayer opponent)
        {
            var points = _formula.UpdatedPoints(0d, this, opponent);

            return new Player(_formula, points, _wins, _defeats + 1);
        }
    }
}