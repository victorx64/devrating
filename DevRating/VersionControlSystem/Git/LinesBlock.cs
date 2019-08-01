using System;

namespace DevRating.VersionControlSystem.Git
{
    public class LinesBlock : IComparable<LinesBlock>
    {
        private readonly int _index;

        private readonly int _length;

        public LinesBlock(int index, int length)
        {
            _index = index;

            _length = length;
        }

        public bool InRange(int index)
        {
            return Start() <= index && index <= End();
        }

        public int Start()
        {
            return _index;
        }

        public int End()
        {
            return _index + _length - 1;
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