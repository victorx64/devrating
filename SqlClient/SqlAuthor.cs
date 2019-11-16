using System.Data;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlClient
{
    internal sealed class SqlAuthor : Author
    {
        private readonly IDbConnection _connection;
        private readonly int _id;

        public SqlAuthor(IDbConnection connection, int id)
        {
            _connection = connection;
            _id = id;
        }

        public int Id()
        {
            return _id;
        }

        public string Email()
        {
            using var command = _connection.CreateCommand();

            command.CommandText = "SELECT [Email] FROM [dbo].[Author] WHERE [Id] = @Id";

            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) {Value = _id});

            using var reader = command.ExecuteReader();

            reader.Read();

            return (string) reader["Email"];
        }
    }
}