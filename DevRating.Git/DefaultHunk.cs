using System;
using System.Collections.Generic;

namespace DevRating.Git
{
    public sealed class DefaultHunk : Hunk
    {
        private readonly string _author;
        private readonly int _index;
        private readonly int _count;

        public DefaultHunk(string author, string header) : this(author, Index(header), Count(header))
        {
        }

        public DefaultHunk(string author, int index, int count)
        {
            _author = author;
            _index = index;
            _count = count;
        }

        public IList<string> DeleteFrom(IEnumerable<string> authors)
        {
            var result = new List<string>(authors);

            if (_count > 0)
            {
                result.RemoveRange(_index, _count);
            }

            return result;
        }

        public IEnumerable<DefaultAuthorChange> ChangedAuthors(IList<string> authors)
        {
            var result = new List<DefaultAuthorChange>();

            for (var i = _index; i < _index + _count; i++)
            {
                if (!authors[i].Equals(_author))
                {
                    result.Add(new DefaultAuthorChange(authors[i], _author)); // TODO remove 'new'
                }
            }

            return result;
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

        public int CompareTo(DefaultHunk other)
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

        private static int Index(string header)
        {
            var parts = header
                .Substring(1)
                .Split(',');

            var index = Convert.ToInt32(parts[0]) - 1;

            if (index > 0)
            {
                return index;
            }

            return 0;
        }

        private static int Count(string header)
        {
            var parts = header
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