using DevRating.Domain;

namespace DevRating.VersionControl
{
    public sealed class VersionControlMatch : Match
    {
        private readonly double _rating;
        private readonly uint _count;

        public VersionControlMatch(double rating, uint count)
        {
            _rating = rating;
            _count = count;
        }

        public double ContenderRating()
        {
            return _rating;
        }

        public uint Count()
        {
            return _count;
        }
    }
}