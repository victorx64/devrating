using System.Collections.Generic;
using System.Linq;
using DevRating.Game;

namespace DevRating.Console
{
    public sealed class DefaultPlayer : Player
    {
        private readonly IList<Game.Game> _games;

        public DefaultPlayer(Game.Game game) : this(new List<Game.Game> {game})
        {
        }

        public DefaultPlayer(IList<Game.Game> games)
        {
            _games = games;
        }

        public Player PerformedPlayer(string contender, string commit, double points, double reward, int rounds)
        {
            return new DefaultPlayer(
                new List<Game.Game>(_games)
                {
                    new DefaultGame(contender, commit, points, reward, rounds)
                });
        }

        public double Points()
        {
            return _games.Last().PointsAfter();
        }

        public IEnumerable<Game.Game> Games()
        {
            return _games;
        }
    }
}