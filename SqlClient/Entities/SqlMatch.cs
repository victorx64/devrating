using System.Data;

namespace DevRating.SqlClient.Entities
{
    internal sealed class SqlMatch : IdentifiableObject
    {
        private readonly IDbConnection _connection;
        private readonly int _id;

        public SqlMatch(IDbConnection connection, int id)
        {
            _connection = connection;
            _id = id;
        }

        public int Id()
        {
            return _id;
        }
    }
}