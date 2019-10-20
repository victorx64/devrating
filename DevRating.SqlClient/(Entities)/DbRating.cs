using System.Data;

namespace DevRating.SqlClient
{
    public class DbRating : Rating
    {
        private readonly int _id;

        public DbRating(IDbConnection connection, int id)
        {
            _id = id;
        }

        public int Id()
        {
            return _id;
        }

        public int AuthorId()
        {
            throw new System.NotImplementedException();
        }

        public int LastRatingId()
        {
            throw new System.NotImplementedException();
        }

        public bool HasLastRating()
        {
            throw new System.NotImplementedException();
        }

        public int MatchId()
        {
            throw new System.NotImplementedException();
        }

        public double Value()
        {
            throw new System.NotImplementedException();
        }
    }
}