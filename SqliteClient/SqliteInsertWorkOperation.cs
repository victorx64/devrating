using System;
using System.Collections.Generic;
using System.Data;
using DevRating.Domain;
using Microsoft.Data.Sqlite;

namespace DevRating.SqliteClient
{
    internal sealed class SqliteInsertWorkOperation : InsertWorkOperation
    {
        private readonly IDbConnection _connection;

        public SqliteInsertWorkOperation(IDbConnection connection)
        {
            _connection = connection;
        }

        public Work Insert(string repository, string start, string end, Entity author, uint additions, Entity rating)
        {
            return Insert(repository, start, end, author.Id(), additions, rating.Id(), DBNull.Value);
        }

        public Work Insert(string repository, string start, string end, Entity author, uint additions)
        {
            return Insert(repository, start, end, author.Id(), additions, DBNull.Value, DBNull.Value);
        }

        public Work Insert(string repository, string start, string end, Entity author, uint additions, Entity rating,
            string link)
        {
            return Insert(repository, start, end, author.Id(), additions, rating.Id(), link);
        }

        public Work Insert(string repository, string start, string end, Entity author, uint additions, string link)
        {
            return Insert(repository, start, end, author.Id(), additions, DBNull.Value, link);
        }

        private Work Insert(object repository, object start, object end, object author, object additions, object rating,
            object link)
        {
            return Insert(new IDbDataParameter[]
            {
                new SqliteParameter("@Repository", SqliteType.Text) {Value = repository},
                new SqliteParameter("@Link", SqliteType.Text) {Value = link},
                new SqliteParameter("@StartCommit", SqliteType.Text, 50) {Value = start},
                new SqliteParameter("@EndCommit", SqliteType.Text, 50) {Value = end},
                new SqliteParameter("@AuthorId", SqliteType.Integer) {Value = author},
                new SqliteParameter("@Additions", SqlDbType.Real) {Value = additions},
                new SqliteParameter("@UsedRatingId", SqliteType.Integer) {Value = rating},
            });
        }

        private Work Insert(IEnumerable<IDbDataParameter> parameters)
        {
            using var command = _connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Work
                    (Repository
                    ,Link
                    ,StartCommit
                    ,EndCommit
                    ,AuthorId
                    ,Additions
                    ,UsedRatingId)
                VALUES
                    (@Repository
                    ,@Link
                    ,@StartCommit
                    ,@EndCommit
                    ,@AuthorId
                    ,@Additions
                    ,@UsedRatingId);
                SELECT last_insert_rowid();";

            foreach (var parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }

            return new SqliteWork(_connection, command.ExecuteScalar());
        }
    }
}