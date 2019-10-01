namespace DevRating.Rating
{
    public class DefaultMatch : Match
    {
        private readonly double _winner;
        private readonly double _loser;
        private readonly int _times;

        public DefaultMatch(double winner, double loser, int times)
        {
            _winner = winner;
            _loser = loser;
            _times = times;
        }
        
        public double Winner()
        {
            return _winner;
        }

        public double Loser()
        {
            return _loser;
        }

        public int Times()
        {
            return _times;
        }
    }
}