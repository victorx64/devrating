using System;
using System.Collections.Generic;
using System.Linq;

namespace DevRating.Console
{
    public sealed class DictionaryPlayers : Players
    {
        private readonly IDictionary<string, Player> _players;

        public DictionaryPlayers() : this(new Dictionary<string, Player>())
        {
        }

        public DictionaryPlayers(IDictionary<string, Player> players)
        {
            _players = players;
        }

        public bool Exists(string name)
        {
            return _players.ContainsKey(name);
        }

        public Player Player(string name)
        {
            return _players[name];
        }

        public void Add(string name, Player player)
        {
            _players.Add(name, player);
        }

        public void Update(string name, Player player)
        {
            if (!Exists(name))
            {
                throw new Exception("Player is not found");
            }
            
            _players[name] = player;
        }
        
        public void PrintToConsole()
        {
            var pairs = _players.ToList();
                
            pairs.Sort(PairComparison);

            foreach (var pair in pairs)
            {
                System.Console.WriteLine($"{pair.Key} {pair.Value.Points()}");
            }
        }

        private int PairComparison(KeyValuePair<string, Player> x, KeyValuePair<string, Player> y)
        {
            return y.Value.Points().CompareTo(x.Value.Points());
        }
    }
}