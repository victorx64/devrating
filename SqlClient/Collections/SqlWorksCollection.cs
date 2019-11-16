using System.Data;
using DevRating.SqlClient.Entities;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlClient.Collections
{
    internal sealed class SqlWorksCollection : WorksCollection
    {
        private readonly IDbConnection _connection;

        public SqlWorksCollection(IDbConnection connection)
        {
            _connection = connection;
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