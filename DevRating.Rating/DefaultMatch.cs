namespace DevRating.Rating
{
    public class DefaultMatch : Match
    {
        private readonly double _winner;
        private readonly double _loser;
        private readonly int _count;

        public DefaultMatch(double winner, double loser, int count)
        {
            _winner = winner;
            _loser = loser;
            _count = count;
        }
        
        public double Winner()
        {
            return _winner;
        }

        public double Loser()
        {
            return _loser;
        }

        public int Count()
        {
            return _count;
        }
    }
}