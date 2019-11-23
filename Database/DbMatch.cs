using DevRating.Domain;

namespace DevRating.Database
{
    public sealed class DbMatch : Match
    {
        private readonly double _rating;
        private readonly uint _count;

        public DbMatch(double rating, uint count)
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