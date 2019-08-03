using System;
using System.Collections.Generic;
using System.Linq;
using DevRating.Rating;

namespace DevRating.Git
{
    public class LinesBlock : IComparable<LinesBlock>
    {
        private readonly string _author;
        private readonly int _index;
        private readonly int _count;

        public LinesBlock(string author, string hunk) : this(author, Index(hunk), Count(hunk))
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

        public IPlayers UpdatedPlayers(IPlayers players, IList<string> authors)
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

        private static int Index(string hunk)
        {
            var parts = hunk
                .Substring(1)
                .Split(',');

            var index = Convert.ToInt32(parts[0]) - 1;

            if (index > 0)
            {
                return index;
            }

            return 0;
        }

        private static int Count(string hunk)
        {
            var parts = hunk
                .Substring(1)
                .Split(',');

            if (parts.Length == 1)
            {
                return 1;
            }

            return Convert.ToInt32(parts[1]);
        }
    }
}