namespace DevRating
{
    public class Developer : IPlayer
    {
        private readonly IPointsFormula _formula;
        private readonly double _points;
        private readonly int _wins;
        private readonly int _defeats;
        private readonly string _id;

        public Developer(IPointsFormula formula, string id) : this(formula, id, 1200, 0, 0)
        {
        }

        public Developer(IPointsFormula formula, string id, double points, int wins, int defeats)
        {
            _formula = formula;
            _points = points;
            _wins = wins;
            _defeats = defeats;
            _id = id;
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

            return new Developer(_formula, _id, points, _wins + 1, _defeats);
        }

        public IPlayer Loser(IPlayer opponent)
        {
            var points = _formula.UpdatedPoints(0d, this, opponent);

            return new Developer(_formula, _id, points, _wins, _defeats + 1);
        }
    }
}