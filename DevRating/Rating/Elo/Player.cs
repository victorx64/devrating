using System;

namespace DevRating.Rating.Elo
{
    public class Player : IComparable<Player>
    {
        private readonly double _points;

        private readonly int _wins;
        
        private readonly int _defeats;

        private readonly string _id;

        public Player(string id) : this(id, 1200, 0, 0)
        {
        }

        private Player(string id, double points, int wins, int defeats)
        {
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

        public Player Winner(double opponentPoints, int opponentGames)
        {
            const double win = 1d;

            var expectation = ExpectedOutcome(opponentPoints);

            var points = UpdatedPoints(win, expectation, opponentGames);

            return new Player(_id, points, _wins + 1, _defeats);
        }

        public Player Loser(double opponentPoints, int opponentGames)
        {
            const double lose = 0d;
            
            var expectation = ExpectedOutcome(opponentPoints);

            var points = UpdatedPoints(lose, expectation, opponentGames);

            return new Player(_id, points, _wins, _defeats + 1);
        }

        public void PrintToConsole()
        {
            Console.WriteLine(FormattableString.Invariant($"{_id}, {_wins}, {_defeats}, {_points:F2}"));
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

            return _points.CompareTo(other._points);
        }

        private double ExpectedOutcome(double opponentPoints)
        {
            var ra = _points;
            
            var rb = opponentPoints;

            const double n = 400d;

            var qa = Math.Pow(10d, ra / n);
            
            var qb = Math.Pow(10d, rb / n);

            var ea = qa / (qa + qb);

            return ea;
        }

        private double UpdatedPoints(double actualOutcome, double expectedOutcome, int opponentGames)
        {
            const int gamesInReplacement = 400;
            
            const double k = 1d;

            return opponentGames < gamesInReplacement
                ? _points
                : _points + k * (actualOutcome - expectedOutcome);
        }
    }
}