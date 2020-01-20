using DevRating.Domain;

namespace DevRating.EloRating.Fake
{
    public sealed class FakeMatch : Match
    {
        private readonly double _rating;
        private readonly uint _count;

        public FakeMatch(double rating, uint count)
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