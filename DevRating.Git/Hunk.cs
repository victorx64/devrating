using System;
using DevRating.Rating;

namespace DevRating.Git
{
    public abstract class Hunk : IComparable<Hunk>
    {
        protected readonly IPlayer Author;
        protected readonly int Index;
        protected readonly int Count;

        protected Hunk(IPlayer author, string header) : this(author, IndexFromHeader(header), CountFromHeader(header))
        {
        }

        protected Hunk(IPlayer author, int index, int count)
        {
            Author = author;
            Index = index;
            Count = count;
        }

        public int CompareTo(Hunk other)
        {
            if (ReferenceEquals(this, other))
            {
                return 0;
            }

            if (ReferenceEquals(null, other))
            {
                return 1;
            }

            return Index.CompareTo(other.Index);
        }

        private static int IndexFromHeader(string header)
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

        private static int CountFromHeader(string header)
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