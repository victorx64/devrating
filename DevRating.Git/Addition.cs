using System;
using System.Collections.Generic;

namespace DevRating.Git
{
    public interface Addition : IComparable<Addition>
    {
        IList<string> AddTo(IEnumerable<string> authors);
    }
}