using System;
using DevRating.DefaultObject;
using Microsoft.Data.Sqlite;
using Xunit;

namespace DevRating.SqliteClient.Test
{
    public sealed class SqliteDatabaseWorkTest
    {
        [Fact]
        public void ReturnsValidAdditions()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var additions = 1u;

                Assert.Equal(
                    additions,
                    database.Entities().Works().InsertOperation().Insert(
                        "repo",
                        "startCommit",
                        "endCommit",
                        database.Entities().Authors().InsertOperation().Insert("email").Id(),
                        additions,
                        new DefaultId(),
                        new DefaultEnvelope()
                    ).Additions()
                );
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsValidAuthor()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var author = database.Entities().Authors().InsertOperation().Insert("email");

                Assert.Equal(
                    author.Id(),
                    database.Entities().Works().InsertOperation().Insert(
                        "repo",
                        "startCommit",
                        "endCommit",
                        author.Id(),
                        2u,
                        new DefaultId(),
                        new DefaultEnvelope()
                    ).Author().Id()
                );
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsValidUsedRating()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var author = database.Entities().Authors().InsertOperation().Insert("email");

                var previous = database.Entities().Ratings().InsertOperation().Insert(
                    3423,
                    new DefaultEnvelope(),
                    new DefaultId(),
                    database.Entities().Works().InsertOperation().Insert(
                        "repo",
                        "startCommit1",
                        "endCommit1",
                        author.Id(),
                        2u,
                        new DefaultId(),
                        new DefaultEnvelope()
                    ).Id(),
                    author.Id()
                );

                Assert.Equal(
                    previous.Id(),
                    database.Entities().Works().InsertOperation().Insert(
                        "repo",
                        "startCommit",
                        "endCommit",
                        author.Id(),
                        2u,
                        previous.Id(),
                        new DefaultEnvelope()
                    ).UsedRating().Id()
                );
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void DoesntImplementToJson()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                Assert.Throws<NotImplementedException>(
                    database.Entities().Works().InsertOperation().Insert(
                        "repo",
                        "startCommit",
                        "endCommit",
                        database.Entities().Authors().InsertOperation().Insert("email").Id(),
                        2u,
                        new DefaultId(),
                        new DefaultEnvelope()
                    ).ToJson
                );
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }
    }
}