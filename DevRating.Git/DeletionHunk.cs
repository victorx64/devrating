using System.Collections.Generic;
using DevRating.Rating;

namespace DevRating.Git
{
    public sealed class DeletionHunk : Hunk, IDeletionHunk
    {
        public DeletionHunk(IPlayer author, string header) : base(author, header)
        {
        }

        public IList<IPlayer> DeleteFrom(IList<IPlayer> authors)
        {
            var output = new List<IPlayer>(authors);

            if (Count > 0)
            {
                output.RemoveRange(Index, Count);
            }

            return output;
        }

        public IList<IPlayer> UpdatedPlayers(IList<IPlayer> players)
        {
            var result = new List<IPlayer>(players);

            for (var i = Index; i < Index + Count; i++)
            {
                var l = players[i];
                var w = Author;

                result.Remove(l);
                result.Remove(w);

                result.Add(l.Loser(w));
                result.Add(w.Winner(l));
            }

            return result;
        }
    }
}