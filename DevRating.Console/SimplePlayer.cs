using System.Collections.Generic;
using System.Linq;

namespace DevRating.Console
{
    public sealed class SimplePlayer : Player
    {
        private readonly IList<Game> _games;

        public SimplePlayer() : this(new List<Game> {new Game()})
        {
        }

        public SimplePlayer(IList<Game> games)
        {
            _games = games;
        }

        public Player NewPlayer(Game game)
        {
            return new SimplePlayer(new List<Game>(_games) {game});
        }

        public double Points()
        {
            return _games.Last().PointsAfter();
        }
    }
}