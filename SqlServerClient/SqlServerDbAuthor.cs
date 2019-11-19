using System.Data;
using DevRating.Database;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlServerClient
{
    public sealed class SqlServerDbAuthor : DbAuthor
    {
        private readonly IDbConnection _connection;
        private readonly int _id;

        public SqlServerDbAuthor(IDbConnection connection, int id)
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

            command.CommandText = "SELECT Email FROM Author WHERE Id = @Id";

            command.Parameters.Add(new SqlParameter("@Id", SqlDbType.Int) {Value = _id});

            using var reader = command.ExecuteReader();

            reader.Read();

            return (string) reader["Email"];
        }
    }
}