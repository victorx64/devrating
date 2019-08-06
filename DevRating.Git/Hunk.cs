using System;
using System.Collections.Generic;

namespace DevRating.Git
{
    public interface Hunk : IComparable<DefaultHunk>
    {
        IList<string> DeleteFrom(IEnumerable<string> authors);
        IEnumerable<DefaultAuthorChange> ChangedAuthors(IList<string> authors);
        IList<string> AddTo(IEnumerable<string> authors);
    }
}