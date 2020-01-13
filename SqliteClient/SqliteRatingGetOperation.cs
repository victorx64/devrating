using System.Data;
using DevRating.Domain;
using Microsoft.Data.Sqlite;

namespace DevRating.SqliteClient
{
    internal sealed class SqliteGetRatingOperation : GetRatingOperation
    {
        private readonly IDbConnection _connection;

        public SqliteGetRatingOperation(IDbConnection connection)
        {
            _connection = connection;
        }

        public Rating RatingOf(Entity author)
        {
            using var command = _connection.CreateCommand();

            command.CommandText =
                "SELECT Id FROM Rating WHERE AuthorId = @AuthorId ORDER BY Id DESC LIMIT 1";

            command.Parameters.Add(new SqliteParameter("@AuthorId", SqliteType.Integer) {Value = author.Id()});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqliteRating(_connection, reader["Id"]);
        }

        public Rating Rating(object id)
        {
            return new SqliteRating(_connection, id);
        }
    }
}