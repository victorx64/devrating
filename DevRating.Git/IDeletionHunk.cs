using System.Collections.Generic;
using DevRating.Rating;

namespace DevRating.Git
{
    public interface IDeletionHunk
    {
        IList<string> DeleteFrom(IEnumerable<string> authors);
        IPlayers UpdatedPlayers(IPlayers players, IList<string> authors);
    }
}