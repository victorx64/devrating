using System.Collections.Generic;
using DevRating.Rating;

namespace DevRating.Git
{
    public sealed class DeletionHunk : Hunk, IDeletionHunk
    {
        public DeletionHunk(string author, string header) : base(author, header)
        {
        }

        public IList<string> DeleteFrom(IEnumerable<string> authors)
        {
            var output = new List<string>(authors);

            if (Count > 0)
            {
                output.RemoveRange(Index, Count);
            }

            return output;
        }

        public IPlayers UpdatedPlayers(IPlayers players, IList<string> authors)
        {
            for (var i = Index; i < Index + Count; i++)
            {
                players = players.UpdatedPlayers(authors[i], Author);
            }

            return players;
        }
    }
}