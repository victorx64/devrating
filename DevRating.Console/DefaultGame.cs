namespace DevRating.Console
{
    public sealed class DefaultGame : Game.Game
    {
        private readonly string _contender;
        private readonly string _commit;
        private readonly double _points;
        private readonly double _reward;
        private readonly int _rounds;

        public DefaultGame(double points) : this(string.Empty, string.Empty, points, 0d, 1)
        {
        }
        
        public DefaultGame(string contender, string commit, double points, double reward, int rounds)
        {
            _contender = contender;
            _commit = commit;
            _points = points;
            _reward = reward;
            _rounds = rounds;
        }

        public double PointsAfter()
        {
            return _points;
        }

        public string Commit()
        {
            return _commit;
        }

        public string Contender()
        {
            return _contender;
        }

        public int Rounds()
        {
            return _rounds;
        }

        public double Reward()
        {
            return _reward;
        }
    }
}