using System;
using System.Collections.Generic;
using System.Linq;

namespace DevRating
{
    public class LinesBlock : IComparable<LinesBlock>
    {
        private readonly string _author;
        private readonly int _index;
        private readonly int _count;

        public LinesBlock(string author, string hunk) : this(author, Index(HunkParts(hunk)[0]),
            Convert.ToInt32(HunkParts(hunk)[1]))
        {
        }

        public LinesBlock(string author, int index, int count)
        {
            _author = author;
            _index = index;
            _count = count;
        }

        public IList<string> AddTo(IEnumerable<string> authors)
        {
            var output = new List<string>(authors);

            for (var i = 0; i < _count; i++)
            {
                output.Insert(_index, _author);
            }

            return output;
        }

        public IList<string> DeleteFrom(IEnumerable<string> authors)
        {
            var output = new List<string>(authors);

            if (_count > 0)
            {
                output.RemoveRange(_index, _count);
            }

            return output;
        }

        public IPlayers UpdateRating(IPlayers players, IList<string> authors)
        {
            for (var i = _index; i < _index + _count; i++)
            {
                players = players.UpdatedPlayers(authors[i], _author);
            }

            return players;
        }

        public int CompareTo(LinesBlock other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (ReferenceEquals(null, other))
            {
                return 1;
            }

            return _index.CompareTo(other._index);
        }

        private static IList<string> HunkParts(string hunk)
        {
            var parts = hunk
                .Substring(1)
                .Split(',')
                .ToList();

            if (parts.Count == 1)
            {
                parts.Add("1");
            }

            return parts;
        }

        private static int Index(string part)
        {
            var index = Convert.ToInt32(part) - 1;

            return index > 0 ? index : 0;
        }
    }
}