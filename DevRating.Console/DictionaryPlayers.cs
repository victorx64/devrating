using System;
using System.Collections.Generic;
using DevRating.Game;

namespace DevRating.Console
{
    public sealed class DictionaryPlayers : Players
    {
        private readonly IDictionary<string, Player> _players;
        private readonly Player _default;

        public DictionaryPlayers(Player @default) : this(new Dictionary<string, Player>(), @default)
        {
        }

        public DictionaryPlayers(IDictionary<string, Player> players, Player @default)
        {
            _players = players;
            _default = @default;
        }

        public Player PlayerOrDefault(string name)
        {
            if (Exists(name))
            {
                return Player(name);
            }

            return _default;
        }

        public void AddOrUpdatePlayer(string name, Player player)
        {
            if (Exists(name))
            {
                Update(name, player);
            }
            else
            {
                Add(name, player);
            }
        }
        
        private bool Exists(string name)
        {
            return _players.ContainsKey(name);
        }
        
        private  Player Player(string name)
        {
            return _players[name];
        }

        private  void Add(string name, Player player)
        {
            _players.Add(name, player);
        }

        private  void Update(string name, Player player)
        {
            if (!Exists(name))
            {
                throw new Exception("Player is not found");
            }
            
            _players[name] = player;
        }
    }
}