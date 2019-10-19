using System.Data;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlClient
{
    internal sealed class DbRatingsCollection : RatingsCollection
    {
        private readonly IDbConnection _connection;

        public DbRatingsCollection(IDbConnection connection)
        {
            _connection = connection;
        }

        public Rating NewRating(int author, double value, int last, int match)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO [dbo].[Rating]
                       ([Rating]
                       ,[LastRatingId]
                       ,[MatchId]
                       ,[AuthorId])
                OUTPUT [Inserted].[Id]
                VALUES
                       (@Rating
                       ,@LastRatingId
                       ,@MatchId
                       ,@AuthorId)";

            command.Parameters.Add(new SqlParameter("@Rating", SqlDbType.Real) {Value = value});
            command.Parameters.Add(new SqlParameter("@LastRatingId", SqlDbType.Int) {Value = last});
            command.Parameters.Add(new SqlParameter("@MatchId", SqlDbType.Int) {Value = match});
            command.Parameters.Add(new SqlParameter("@AuthorId", SqlDbType.Int) {Value = author});

            return new DbRating(_connection, (int) command.ExecuteScalar());
        }

        public Rating LastRatingOf(int author)
        {
            throw new System.NotImplementedException();
        }
    }
}