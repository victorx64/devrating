using System.Collections.Generic;
using System.Linq;

namespace DevRating.Game
{
    public sealed class DefaultPlayer : Player
    {
        private readonly IList<Game> _games;

        public DefaultPlayer(Game game) : this(new List<Game> {game})
        {
        }

        public DefaultPlayer(IList<Game> games)
        {
            _games = games;
        }

        public Player NewPlayer(Game game)
        {
            return new DefaultPlayer(new List<Game>(_games) {game});
        }

        public double Points()
        {
            return _games.Last().PointsAfter();
        }
    }
}