using System.Collections.Generic;
using System.Linq;
using DevRating.Git;
using DevRating.Rating;

namespace DevRating.Console
{
    internal class Players : AuthorsLog
    {
        private readonly IDictionary<string, Player> _players;
        private readonly Player _new;
        private readonly Player _empty;

        public Players() : this(new Dictionary<string, Player>(), new DefaultPlayer(), new DefaultPlayer())
        {
        }

        public Players(IDictionary<string, Player> players, Player @new, Player empty)
        {
            _players = players;
            _new = @new;
            _empty = empty;
        }

        public void LogAuthorDeletion(string author, string deleter, string commit)
        {
            if (author.Equals(deleter))
                return;
            
            lock (_new)
            {
                var previous = RemovedPlayer(author);
                var next = RemovedPlayer(deleter);

                var loser = previous.Loser(next);
                var winner = next.Winner(previous);

                _players.Add(author, loser);
                _players.Add(deleter, winner);
            }
        }

        public void LogAuthorAddition(string author, string commit)
        {
            lock (_new)
            {
                var winner = RemovedPlayer(author).Winner(_empty);

                _players.Add(author, winner);
            }
        }

        public void PrintToConsole()
        {
            lock (_new)
            {
                var pairs = _players.ToList();
                    
                pairs.Sort(PairComparison);

                foreach (var pair in pairs)
                {
                    System.Console.WriteLine($"{pair.Key} {pair.Value.Points()}");
                }
            }
        }

        private int PairComparison(KeyValuePair<string, Player> x, KeyValuePair<string, Player> y)
        {
            return x.Value.Points().CompareTo(y.Value.Points());
        }

        private Player RemovedPlayer(string key)
        {
            Player player;

            if (_players.ContainsKey(key))
                _players.Remove(key, out player);
            else
                player = _new;

            return player;
        }
    }
}