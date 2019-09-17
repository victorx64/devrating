using DevRating.Game;

namespace DevRating.Console
{
    public sealed class DefaultPlayer : Player
    {
        private readonly double _points;

        public DefaultPlayer(double points) 
        {
            _points = points;
        }

        public Player PerformedPlayer(string contender, string commit, double points, double reward, int rounds)
        {
            return new DefaultPlayer(points);
        }

        public double Points()
        {
            return _points;
        }
    }
}