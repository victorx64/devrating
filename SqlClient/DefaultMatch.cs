using DevRating.Domain;

namespace DevRating.SqlClient
{
    internal sealed class DefaultMatch : Match
    {
        private readonly double _rating;
        private readonly uint _count;

        public DefaultMatch(double rating, uint count)
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