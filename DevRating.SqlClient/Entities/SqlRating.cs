using System.Data;
using DevRating.Domain;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlClient.Entities
{
    internal sealed class SqlRating : Domain.Rating, IdentifiableObject
    {
        private readonly IDbTransaction _transaction;
        private readonly int _id;

        public SqlRating(IDbTransaction transaction, int id)
        {
            _transaction = transaction;
            _id = id;
        }

        public int Id()
        {
            return _id;
        }

//        public int LastRatingId()
//        {
//            using var command = _transaction.Connection.CreateCommand();
//            command.Transaction = _transaction;
//
//            command.CommandText = "SELECT [LastRatingId] FROM [dbo].[Rating] WHERE [Id] = @Id";
//
//            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) {Value = _id});
//
//            using var reader = command.ExecuteReader();
//
//            reader.Read();
//
//            return (int) reader["LastRatingId"];
//        }
//
//        public bool HasLastRating()
//        {
//            using var command = _transaction.Connection.CreateCommand();
//            command.Transaction = _transaction;
//
//            command.CommandText = "SELECT [LastRatingId] FROM [dbo].[Rating] WHERE [Id] = @Id";
//
//            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) {Value = _id});
//
//            using var reader = command.ExecuteReader();
//
//            return reader.Read();
//        }
//
//        public int MatchId()
//        {
//            using var command = _transaction.Connection.CreateCommand();
//            command.Transaction = _transaction;
//
//            command.CommandText = "SELECT [MatchId] FROM [dbo].[Rating] WHERE [Id] = @Id";
//
//            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) {Value = _id});
//
//            using var reader = command.ExecuteReader();
//
//            reader.Read();
//
//            return (int) reader["MatchId"];
//        }

        public Author Author()
        {
            using var command = _transaction.Connection.CreateCommand();
            command.Transaction = _transaction;

            command.CommandText = "SELECT [AuthorId] FROM [dbo].[Rating] WHERE [Id] = @Id";

            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) {Value = _id});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqlAuthor(_transaction, (int) reader["AuthorId"]);
        }

        public double Value()
        {
            using var command = _transaction.Connection.CreateCommand();
            command.Transaction = _transaction;

            command.CommandText = "SELECT [Rating] FROM [dbo].[Rating] WHERE [Id] = @Id";

            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) {Value = _id});

            using var reader = command.ExecuteReader();

            reader.Read();

            return (float) reader["Rating"];
        }
    }
}