using System.Collections.Generic;
using System.Linq;
using DevRating.Git;
using DevRating.Rating;

namespace DevRating.Console
{
    internal class Players : ChangeLog
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

        public void LogDeletion(string victim, string initiator, string commit)
        {
            if (victim.Equals(initiator))
                return;
            
            lock (_new)
            {
                var previous = RemovedPlayer(victim);
                var next = RemovedPlayer(initiator);

                var loser = previous.Loser(next);
                var winner = next.Winner(previous);

                _players.Add(victim, loser);
                _players.Add(initiator, winner);
            }
        }

        public void LogAddition(string initiator, string commit)
        {
//            lock (_new)
//            {
//                var winner = RemovedPlayer(author).Winner(_empty);
//
//                _players.Add(author, winner);
//            }
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