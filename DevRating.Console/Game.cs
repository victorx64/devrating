namespace DevRating.Console
{
    public sealed class Game
    {
        private readonly double _points;

        public Game(string contender, double points) : this(contender, string.Empty, points, 0d, 1)
        {
        }
        
        public Game(string contender, string commit, double points, double reward, int rounds)
        {
            _points = points;
        }

        public double PointsAfter()
        {
            return _points;
        }
    }
}