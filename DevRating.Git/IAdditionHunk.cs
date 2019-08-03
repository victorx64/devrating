using System.Collections.Generic;

namespace DevRating.Git
{
    public interface IAdditionHunk
    {
        IList<string> AddTo(IEnumerable<string> authors);
    }
}