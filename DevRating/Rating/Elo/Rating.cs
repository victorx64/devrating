using System;
using System.Collections.Generic;

namespace DevRating.Rating.Elo
{
    public class Rating : IRating
    {
        private readonly Dictionary<string, Player> _players;

        public Rating() : this(new Dictionary<string, Player>()) { }

        private Rating(Dictionary<string, Player> points)
        {
            _players = points;
        }
        
        public IRating Update(string loserId, string winnerId)
        {
            var loser = _players.ContainsKey(loserId) ? _players[loserId] : new Player();
            
            var winner = _players.ContainsKey(winnerId) ? _players[winnerId] : new Player();

            var players = new Dictionary<string, Player>(_players)
            {
                [loserId] = loser.Lose(winner.Points(), winner.Games()), 
                [winnerId] = winner.Win(loser.Points(), loser.Games())
            };

            return new Rating(players);
        }

        public void PrintToConsole()
        {
            foreach (var player in _players)
            {
                Console.WriteLine($"{player.Key} ");
                
                player.Value.PrintToConsole();
            }
        }
    }
}