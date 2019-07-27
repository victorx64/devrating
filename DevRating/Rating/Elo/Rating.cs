using System;
using System.Collections.Generic;
using System.Linq;

namespace DevRating.Rating.Elo
{
    public class Rating : IRating
    {
        private readonly Dictionary<string, Player> _players;

        public Rating() : this(new Dictionary<string, Player>())
        {
        }

        private Rating(Dictionary<string, Player> players)
        {
            _players = players;
        }

        public IRating UpdatedRating(string loserId, string winnerId)
        {
            if (loserId.Equals(winnerId))
            {
                return this;
            }

            var loser = _players.ContainsKey(loserId) ? _players[loserId] : new Player(loserId);

            var winner = _players.ContainsKey(winnerId) ? _players[winnerId] : new Player(winnerId);

            var players = new Dictionary<string, Player>(_players)
            {
                [loserId] = loser.Loser(winner.Points(), winner.Games()),

                [winnerId] = winner.Winner(loser.Points(), loser.Games())
            };

            return new Rating(players);
        }

        public void PrintToConsole()
        {
            var players = _players.Values.ToList();

            players.Sort();

            players.Reverse();

            Console.WriteLine("Player, Wins, Defeats, Points");

            foreach (var player in players)
            {
                player.PrintToConsole();
            }
        }
    }
}