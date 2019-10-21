using System.Data;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlClient
{
    internal sealed class DbAuthor : Author
    {
        private readonly IDbTransaction _transaction;
        private readonly int _id;

        public DbAuthor(IDbTransaction transaction, int id)
        {
            _transaction = transaction;
            _id = id;
        }

        public int Id()
        {
            return _id;
        }

        public string Email()
        {
            using var command = _transaction.Connection.CreateCommand();
            command.Transaction = _transaction;

            command.CommandText = "SELECT [Email] FROM [dbo].[Author] WHERE [Id] = @Id";

            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) {Value = _id});

            using var reader = command.ExecuteReader();

            reader.Read();

            return (string) reader["Email"];
        }
    }
}