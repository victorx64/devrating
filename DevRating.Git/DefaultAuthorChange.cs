using System.Collections.Generic;
using DevRating.Rating;

namespace DevRating.Git
{
    public sealed class DefaultAuthorChange : AuthorChange
    {
        private readonly string _previous;
        private readonly string _next;
        private readonly Player _initial;

        public DefaultAuthorChange(string previous, string next, Player initial)
        {
            _previous = previous;
            _next = next;
            _initial = initial;
        }

        public IDictionary<string, Player> UpdatedAuthors(IDictionary<string, Player> authors)
        {
            var result = authors; // new Dictionary<string, IPlayer>(authors);
            
            Player previous, next;

            if (result.ContainsKey(_previous))
            {
                result.Remove(_previous, out previous);
            }
            else
            {
                previous = _initial;
            }
            
            if (result.ContainsKey(_next))
            {
                result.Remove(_next, out next);
            }
            else
            {
                next = _initial;
            }

            var loser = previous.Loser(next);
            var winner = next.Winner(previous);
            
            result.Add(_previous, loser);
            result.Add(_next, winner);

            return result;
        }
    }
}