using System;

namespace DevRating.Rating.Elo
{
    public class Player
    {
        private readonly double _points;
        private readonly int _games;

        public Player() : this(1000, 0) { }

        private Player(double points, int games)
        {
            _points = points;
            _games = games;
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
            const double winOutcome = 1d;
            
            return UpdatedPlayer(winOutcome, opponentPoints, opponentGames);
        }

        public Player Lose(double opponentPoints, int opponentGames)
        {
            const double loseOutcome = 0d;
            
            return UpdatedPlayer(loseOutcome, opponentPoints, opponentGames);
        }

        public void PrintToConsole()
        {
            Console.Write($"{_points} {_games}");
        }

        private Player UpdatedPlayer(double actualOutcome, double opponentPoints, int opponentGames)
        {
            const int gamesInReplacement = 400;
            
            var points = opponentGames < gamesInReplacement
                ? _points 
                : UpdatedPoints(actualOutcome, opponentPoints);

            return new Player(points, _games + 1);
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

        private double UpdatedPoints(double actualOutcome, double opponentPoints)
        {
            const double k = 1d;
            
            return _points + k * (actualOutcome - ExpectedOutcome(opponentPoints));
        }
    }
}