namespace DevRating.Console
{
    public sealed class DefaultGame : Game
    {
        private readonly double _points;

        public DefaultGame(double points) : this(string.Empty, string.Empty, points, 0d, 1)
        {
        }
        
        public DefaultGame(string contender, string commit, double points, double reward, int rounds)
        {
            _points = points;
        }

        public double PointsAfter()
        {
            return _points;
        }
    }
}