using System.Data;
using DevRating.Domain;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlClient
{
    internal sealed class SqlWorks : Works
    {
        private readonly IDbConnection _connection;

        public SqlWorks(IDbConnection connection)
        {
            _connection = connection;
        }

        public IdentifiableWork Work(WorkKey key)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                SELECT Id 
                FROM Work 
                WHERE Repository = @Repository 
                AND StartCommit = @StartCommit
                AND EndCommit = @EndCommit";

            command.Parameters.Add(new SqlParameter("@Repository", SqlDbType.NVarChar) {Value = key.Repository()});
            command.Parameters.Add(new SqlParameter("@StartCommit", SqlDbType.NVarChar, 50)
                {Value = key.StartCommit()});
            command.Parameters.Add(new SqlParameter("@EndCommit", SqlDbType.NVarChar, 50) {Value = key.EndCommit()});

            using var reader = command.ExecuteReader();

            reader.Read();

            return new SqlWork(_connection, (int) reader["Id"]);
        }

        public bool Exist(WorkKey key)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                SELECT Id 
                FROM Work 
                WHERE Repository = @Repository 
                AND StartCommit = @StartCommit
                AND EndCommit = @EndCommit";

            command.Parameters.Add(new SqlParameter("@Repository", SqlDbType.NVarChar) {Value = key.Repository()});
            command.Parameters.Add(new SqlParameter("@StartCommit", SqlDbType.NVarChar, 50)
                {Value = key.StartCommit()});
            command.Parameters.Add(new SqlParameter("@EndCommit", SqlDbType.NVarChar, 50) {Value = key.EndCommit()});

            using var reader = command.ExecuteReader();

            return reader.Read();
        }

        public IdentifiableWork Insert(string repository, string start, string end, IdentifiableObject author,
            double reward, IdentifiableObject rating)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO [dbo].[Work]
                       ([Repository]
                       ,[StartCommit]
                       ,[EndCommit]
                       ,[AuthorId]
                       ,[Reward]
                       ,[UsedRatingId])
                OUTPUT [Inserted].[Id]
                VALUES
                       (@Repository
                       ,@StartCommit
                       ,@EndCommit
                       ,@AuthorId
                       ,@Reward
                       ,@UsedRatingId)";

            command.Parameters.Add(new SqlParameter("@Repository", SqlDbType.NVarChar) {Value = repository});
            command.Parameters.Add(new SqlParameter("@StartCommit", SqlDbType.NVarChar, 50) {Value = start});
            command.Parameters.Add(new SqlParameter("@EndCommit", SqlDbType.NVarChar, 50) {Value = end});
            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = author.Id()});
            command.Parameters.Add(new SqlParameter("@Reward", SqlDbType.Real) {Value = reward});
            command.Parameters.Add(new SqlParameter("@UsedRatingId", SqlDbType.Int) {Value = rating.Id()});

            var id = (int) command.ExecuteScalar();

            return new SqlWork(_connection, id);
        }

        public IdentifiableWork Insert(string repository, string start, string end, IdentifiableObject author,
            double reward)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO [dbo].[Work]
                       ([Repository]
                       ,[StartCommit]
                       ,[EndCommit]
                       ,[AuthorId]
                       ,[Reward]
                       ,[UsedRatingId])
                OUTPUT [Inserted].[Id]
                VALUES
                       (@Repository
                       ,@StartCommit
                       ,@EndCommit
                       ,@AuthorId
                       ,@Reward
                       ,NULL)";

            command.Parameters.Add(new SqlParameter("@Repository", SqlDbType.NVarChar) {Value = repository});
            command.Parameters.Add(new SqlParameter("@StartCommit", SqlDbType.NVarChar, 50) {Value = start});
            command.Parameters.Add(new SqlParameter("@EndCommit", SqlDbType.NVarChar, 50) {Value = end});
            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = author.Id()});
            command.Parameters.Add(new SqlParameter("@Reward", SqlDbType.Real) {Value = reward});

            var id = (int) command.ExecuteScalar();

            return new SqlWork(_connection, id);
        }
    }
}