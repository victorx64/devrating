using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevRating.Game;

namespace DevRating.InMemoryStorage
{
    public class InMemoryMatches : Matches
    {
        private readonly IDictionary<string, double> _players;
        private readonly double _points;
        
        public InMemoryMatches(double points) : this(new Dictionary<string, double>(), points)
        {
        }

        public InMemoryMatches(IDictionary<string, double> players, double points)
        {
            _players = players;
            _points = points;
        }

        public Task<double> Points(string player)
        {
            if (_players.ContainsKey(player))
            {
                return Task.FromResult(_players[player]);
            }

            return Task.FromResult(_points);
        }

        public Task Add(string player, string contender, string commit, double points, double reward, int rounds)
        {
            if (_players.ContainsKey(player))
            {
                _players[player] = points;
            }
            else
            {
                _players.Add(player, points);
            }

            return Task.CompletedTask;
        }

        public Task Add(string player, string commit, double points, double reward, int rounds)
        {
            return Add(player, string.Empty, commit, points, reward, rounds);
        }

        public Task<IEnumerable<Match>> Matches(string player)
        {
            throw new System.NotImplementedException();
        }

        public Task Lock(string player)
        {
            return Task.CompletedTask;
        }

        public Task Unlock(string player)
        {
            return Task.CompletedTask;
        }

        public Task Sync()
        {
            return Task.CompletedTask;
        }

        public void PrintToConsole()
        {
            var players = _players.ToList();
            
            players.Sort(Comparison);

            foreach (var pair in players)
            {
                Console.WriteLine($"{pair.Key} {pair.Value}");
            }
        }

        private int Comparison(KeyValuePair<string, double> x, KeyValuePair<string, double> y)
        {
            return y.Value.CompareTo(x.Value);
        }
    }
}