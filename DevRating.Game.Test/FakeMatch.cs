namespace DevRating.Game.Test
{
    public class FakeMatch : Match
    {
        private readonly string _player;
        private readonly string _contender;
        private readonly double _points;
        private readonly int _rounds;

        public FakeMatch(string player, string contender, double points, int rounds)
        {
            _player = player;
            _contender = contender;
            _points = points;
            _rounds = rounds;
        }
        
        public string Player()
        {
            return _player;
        }

        public double Points()
        {
            return _points;
        }

        public string Commit()
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }
    }
}