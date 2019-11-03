using System;
using System.Data;
using DevRating.SqlClient.Entities;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlClient.Collections
{
    internal sealed class SqlMatchesCollection : MatchesCollection
    {
        private readonly IDbTransaction _transaction;

        public SqlMatchesCollection(IDbTransaction transaction)
        {
            _transaction = transaction;
        }

        public SqlMatch NewMatch(int first, int second, string commit, string repository, uint count)
        {
            if (first.Equals(second))
            {
                throw new Exception($"Params {nameof(first)} and {nameof(second)} must not be the same.");
            }

            using var command = _transaction.Connection.CreateCommand();
            command.Transaction = _transaction;
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

            return new SqlMatch(_transaction, (int) command.ExecuteScalar());
        }
    }
}