using System;
using System.Collections.Generic;

namespace DevRating.Git
{
    public interface Deletion : IComparable<Deletion>
    {
        IList<string> DeleteFrom(IEnumerable<string> authors);
        IEnumerable<AuthorChange> ChangedAuthors(IList<string> authors);
    }
}