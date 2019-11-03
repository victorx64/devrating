using System.Data;

namespace DevRating.SqlClient.Entities
{
    internal sealed class SqlMatch : IdentifiableObject
    {
        private readonly IDbTransaction _transaction;
        private readonly int _id;

        public SqlMatch(IDbTransaction transaction, int id)
        {
            _transaction = transaction;
            _id = id;
        }

        public int Id()
        {
            return _id;
        }
    }
}