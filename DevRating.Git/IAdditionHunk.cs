using System.Collections.Generic;
using DevRating.Rating;

namespace DevRating.Git
{
    public interface IAdditionHunk
    {
        IList<IPlayer> AddTo(IEnumerable<IPlayer> authors);
    }
}