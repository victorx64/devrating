using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevRating.Game.Test
{
    public class FakeMatches : Matches
    {
        private readonly double _points;
        private readonly IDictionary<string, IList<Match>> _players;

        public FakeMatches(double points)
        {
            _points = points;
            _players = new Dictionary<string, IList<Match>>();
        }

        public Task<double> Points(string player)
        {
            if (_players.ContainsKey(player))
            {
                return Task.FromResult(_players[player].Last().Points());
            }
            else
            {
                return Task.FromResult(_points);
            }
        }

        public Task Add(string player, string contender, string commit, double points, double reward, int rounds)
        {
            if (_players.ContainsKey(player))
            {
                _players[player].Add(new FakeMatch(player, contender, points, rounds));
            }
            else
            {
                _players[player] = new List<Match>
                {
                    new FakeMatch(player, contender, points, rounds)
                };
            }

            return Task.CompletedTask;
        }

        public Task Add(string player, string commit, double points, double reward, int rounds)
        {
            return Add(player, string.Empty, commit, points, reward, rounds);
        }

        public Task<IEnumerable<Match>> Matches(string player)
        {
            return Task.FromResult((IEnumerable<Match>) _players[player]);
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
    }
}