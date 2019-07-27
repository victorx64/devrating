using System;

namespace DevRating.Rating.Elo
{
    public class Player : IComparable<Player>
    {
        private readonly double _points;

        private readonly int _games;

        private readonly string _id;

        public Player(string id) : this(id, 1000, 0)
        {
        }

        private Player(string id, double points, int games)
        {
            _points = points;
            
            _games = games;
            
            _id = id;
        }

        public double Points()
        {
            return _points;
        }

        public int Games()
        {
            return _games;
        }

        public Player Win(double opponentPoints, int opponentGames)
        {
            const double win = 1d;

            var expectation = ExpectedOutcome(opponentPoints);

            return UpdatedPlayer(win, expectation, opponentGames);
        }

        public Player Lose(double opponentPoints, int opponentGames)
        {
            const double lose = 0d;
            
            var expectation = ExpectedOutcome(opponentPoints);

            return UpdatedPlayer(lose, expectation, opponentGames);
        }

        public void PrintToConsole()
        {
            Console.WriteLine(FormattableString.Invariant($"{_id}, {_points:F2}, {_games}"));
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

        private Player UpdatedPlayer(double actualOutcome, double expectedOutcome, int opponentGames)
        {
            const int gamesInReplacement = 400;

            var points = opponentGames < gamesInReplacement
                ? _points
                : UpdatedPoints(actualOutcome, expectedOutcome);

            return new Player(_id, points, _games + 1);
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

        private double UpdatedPoints(double actualOutcome, double expectedOutcome)
        {
            const double k = 1d;

            return _points + k * (actualOutcome - expectedOutcome);
        }
    }
}