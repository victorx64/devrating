using System.Data;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlClient
{
    internal sealed class DbMatchesCollection : MatchesCollection
    {
        private readonly IDbConnection _connection;

        public DbMatchesCollection(IDbConnection connection)
        {
            _connection = connection;
        }

        public Match NewMatch(int first, int second, string commit, string repository, int count)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO [dbo].[Match]
                       ([FirstAuthorId]
                       ,[SecondAuthorId]
                       ,[Commit]
                       ,[Repository]
                       ,[Count])
                OUTPUT [Inserted].[Id]
                VALUES
                       (@FirstAuthorId
                       ,@SecondAuthorId
                       ,@Commit
                       ,@Repository
                       ,@Count)";

            command.Parameters.Add(new SqlParameter("@FirstAuthorId", SqlDbType.Int) {Value = first});
            command.Parameters.Add(new SqlParameter("@SecondAuthorId", SqlDbType.Int) {Value = second});
            command.Parameters.Add(new SqlParameter("@Commit", SqlDbType.NVarChar, 50) {Value = commit});
            command.Parameters.Add(new SqlParameter("@Repository", SqlDbType.NVarChar) {Value = repository});
            command.Parameters.Add(new SqlParameter("@Count", SqlDbType.Int) {Value = count});

            return new DbMatch(_connection, (int) command.ExecuteScalar());
        }
    }
}