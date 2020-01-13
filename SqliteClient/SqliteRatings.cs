using System.Data;
using DevRating.Domain;
using Microsoft.Data.Sqlite;

namespace DevRating.SqliteClient
{
    internal sealed class SqliteRatings : Ratings
    {
        private readonly IDbConnection _connection;
        private readonly InsertRatingOperation _insert;
        private readonly GetRatingOperation _get;

        public SqliteRatings(IDbConnection connection)
            : this(new SqliteInsertRatingOperation(connection),
                new SqliteGetRatingOperation(connection))
        {
        }

        public SqliteRatings(InsertRatingOperation insert, GetRatingOperation get)
        {
            _insert = insert;
            _get = get;
        }

        public InsertRatingOperation InsertOperation()
        {
            return _insert;
        }

        public GetRatingOperation GetOperation()
        {
            return _get;
        }


        public bool Contains(object id)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT Id FROM Rating WHERE Id = @Id";

            command.Parameters.Add(new SqliteParameter("@Id", SqliteType.Integer) {Value = id});

            using var reader = command.ExecuteReader();

            return reader.Read();
        }

        public bool ContainsRatingOf(Entity author)
        {
            using var command = _connection.CreateCommand();

            command.CommandText =
                "SELECT Id FROM Rating WHERE AuthorId = @AuthorId ORDER BY Id DESC LIMIT 1";

            command.Parameters.Add(new SqliteParameter("@AuthorId", SqliteType.Integer) {Value = author.Id()});

            using var reader = command.ExecuteReader();

            return reader.Read();
        }
    }
}