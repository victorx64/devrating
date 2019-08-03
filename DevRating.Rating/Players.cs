using System.Collections.Generic;

namespace DevRating.Rating
{
    public class Players : IPlayers
    {
        private readonly IDictionary<string, IPlayer> _players;

        public Players() : this(new Dictionary<string, IPlayer>())
        {
        }

        public Players(IDictionary<string, IPlayer> players)
        {
            _players = players;
        }

        public bool Exist(string id)
        {
            return _players.ContainsKey(id);
        }

        public void Add(string id, IPlayer player)
        {
            _players.Add(id, player);
        }

        public IPlayers UpdatedPlayers(string loser, string winner)
        {
            if (loser.Equals(winner))
            {
                return this;
            }

            var l = _players[loser];
            var w = _players[winner];

            var players = new Dictionary<string, IPlayer>(_players)
            {
                [loser] = l.Loser(w),
                [winner] = w.Winner(l)
            };

            return new Players(players);
        }
    }
}