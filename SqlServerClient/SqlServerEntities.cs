using System.Data;
using DevRating.Database;

namespace DevRating.SqlServerClient
{
    public sealed class SqlServerEntities : Entities
    {
        private readonly IDbConnection _connection;

        public SqlServerEntities(IDbConnection connection)
        {
            _connection = connection;
        }

        public IDbConnection Connection()
        {
            return _connection;
        }

        public Works Works()
        {
            return new SqlServerWorks(_connection);
        }

        public Authors Authors()
        {
            return new SqlServerAuthors(_connection);
        }

        public Ratings Ratings()
        {
            return new SqlServerRatings(_connection);
        }
    }
}