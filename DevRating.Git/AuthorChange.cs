using System.Collections.Generic;
using DevRating.Rating;

namespace DevRating.Git
{
    public sealed class AuthorChange
    {
        private readonly string _previous;
        private readonly string _next;

        public AuthorChange(string previous, string next)
        {
            _previous = previous;
            _next = next;
        }

        public IDictionary<string, IPlayer> UpdatedAuthors(IDictionary<string, IPlayer> authors, IPlayer initial)
        {
            var result = new Dictionary<string, IPlayer>(authors);
            
            IPlayer previous, next;

            if (result.ContainsKey(_previous))
            {
                result.Remove(_previous, out previous);
            }
            else
            {
                previous = initial;
            }
            
            if (result.ContainsKey(_next))
            {
                result.Remove(_next, out next);
            }
            else
            {
                next = initial;
            }

            var loser = previous.Loser(next);
            var winner = next.Winner(previous);
            
            result.Add(_previous, loser);
            result.Add(_next, winner);

            return result;
        }
    }
}