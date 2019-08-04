using System.Collections.Generic;
using DevRating.Rating;

namespace DevRating.Git
{
    public sealed class AdditionHunk : Hunk, IAdditionHunk
    {
        public AdditionHunk(IPlayer author, string header) : base(author, header)
        {
        }

        public IList<IPlayer> AddTo(IEnumerable<IPlayer> authors)
        {
            var output = new List<IPlayer>(authors);

            for (var i = 0; i < Count; i++)
            {
                output.Insert(Index, Author);
            }

            return output;
        }
    }
}