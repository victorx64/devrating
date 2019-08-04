using System.Collections.Generic;
using DevRating.Rating;

namespace DevRating.Git
{
    public interface IDeletionHunk
    {
        IList<IPlayer> DeleteFrom(IList<IPlayer> authors);
        IList<IPlayer> UpdatedPlayers(IList<IPlayer> players);
    }
}