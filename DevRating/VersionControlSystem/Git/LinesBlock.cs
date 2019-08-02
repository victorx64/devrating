using System;
using System.Collections.Generic;
using DevRating.Rating;

namespace DevRating.VersionControlSystem.Git
{
    public class LinesBlock : IComparable<LinesBlock>
    {
        private readonly string _author;
        private readonly int _index;
        private readonly int _count;

        public LinesBlock(string author, string hunkHeader)
        {
            _author = author;
            
            var parts = hunkHeader.Substring(1).Split(',');

            var oneBasedIndex = Convert.ToInt32(parts[0]);

            _index = oneBasedIndex == 0 ? 0 : oneBasedIndex - 1;

            _count = parts.Length > 1 ? Convert.ToInt32(parts[1]) : 1;
        }

        public IEnumerable<string> AddTo(IEnumerable<string> authors)
        {
            var output = new List<string>(authors);

            for (var i = 0; i < _count; i++)
            {
                output.Insert(_index, _author);
            }

            return output;
        }
        
        public IEnumerable<string> DeleteFrom(IEnumerable<string> authors)
        {
            var output = new List<string>(authors);

            if (_count > 0)
            {
                output.RemoveRange(_index, _count);
            }

            return output;
        }

        public IRating UpdateRating(IRating rating, IEnumerable<string> authors)
        {
            var list = new List<string>(authors);
            
            for (var i = _index; i < _index + _count; i++)
            {
                rating = rating.UpdatedRating(list[i], _author);
            }

            return rating;
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
    }
}