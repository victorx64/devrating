using System;
using System.Collections.Generic;

namespace DevRating.Git
{
    public interface IAdditionHunk : IComparable<IAdditionHunk>
    {
        IList<string> AddTo(IEnumerable<string> authors);
    }
}