using System;
using DevRating.DefaultObject;
using Microsoft.Data.Sqlite;
using Xunit;

namespace DevRating.SqliteClient.Test
{
    public sealed class SqliteDatabaseRatingTest
    {
        [Fact]
        public void ReturnsValidValue()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var author = database.Entities().Authors().InsertOperation().Insert("email");

                var value = 1100d;

                Assert.Equal(
                    value,
                    database.Entities().Ratings().InsertOperation().Insert(
                        value,
                        new DefaultEnvelope(),
                        new DefaultId(),
                        database.Entities().Works().InsertOperation().Insert(
                            "repo",
                            "startCommit",
                            "endCommit",
                            author.Id(),
                            1u,
                            new DefaultId(),
                            new DefaultEnvelope()
                        ).Id(),
                        author.Id()
                    ).Value()
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

                var value = 1100d;

                Assert.Equal(
                    author.Id(),
                    database.Entities().Ratings().InsertOperation().Insert(
                        value,
                        new DefaultEnvelope(),
                        new DefaultId(),
                        database.Entities().Works().InsertOperation().Insert(
                            "repo",
                            "startCommit",
                            "endCommit",
                            author.Id(),
                            1u,
                            new DefaultId(),
                            new DefaultEnvelope()
                        ).Id(),
                        author.Id()
                    ).Author().Id()
                );
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsValidWork()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var author = database.Entities().Authors().InsertOperation().Insert("email");

                var work = database.Entities().Works().InsertOperation().Insert(
                    "repo",
                    "startCommit",
                    "endCommit",
                    author.Id(),
                    1u,
                    new DefaultId(),
                    new DefaultEnvelope()
                );

                Assert.Equal(
                    work.Id(),
                    database.Entities().Ratings().InsertOperation().Insert(
                        1100d,
                        new DefaultEnvelope(),
                        new DefaultId(),
                        work.Id(),
                        author.Id()
                    ).Work().Id()
                );
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsValidDeletions()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var author = database.Entities().Authors().InsertOperation().Insert("email");

                var deletions = new DefaultEnvelope(123123L);

                Assert.Equal(
                    deletions.Value(),
                    database.Entities().Ratings().InsertOperation().Insert(
                        1100d,
                        deletions,
                        new DefaultId(),
                        database.Entities().Works().InsertOperation().Insert(
                            "repo",
                            "startCommit",
                            "endCommit",
                            author.Id(),
                            1u,
                            new DefaultId(),
                            new DefaultEnvelope()
                        ).Id(),
                        author.Id()
                    ).Deletions().Value()
                );
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsValidPreviousRating()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var author = database.Entities().Authors().InsertOperation().Insert("email");

                var previous = database.Entities().Ratings().InsertOperation().Insert(
                    12d,
                    new DefaultEnvelope(),
                    new DefaultId(),
                    database.Entities().Works().InsertOperation().Insert(
                        "repo",
                        "startCommit1",
                        "endCommit1",
                        author.Id(),
                        12u,
                        new DefaultId(),
                        new DefaultEnvelope()
                    ).Id(),
                    author.Id()
                );

                Assert.Equal(
                    previous.Id(),
                    database.Entities().Ratings().InsertOperation().Insert(
                        1100d,
                        new DefaultEnvelope(),
                        previous.Id(),
                        database.Entities().Works().InsertOperation().Insert(
                            "repo",
                            "startCommit2",
                            "endCommit2",
                            author.Id(),
                            1u,
                            new DefaultId(),
                            new DefaultEnvelope()
                        ).Id(),
                        author.Id()
                    ).PreviousRating().Id()
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
                var author = database.Entities().Authors().InsertOperation().Insert("email");

                Assert.Throws<NotImplementedException>(database.Entities().Ratings().InsertOperation().Insert(
                        12d,
                        new DefaultEnvelope(),
                        new DefaultId(),
                        database.Entities().Works().InsertOperation().Insert(
                            "repo",
                            "startCommit1",
                            "endCommit1",
                            author.Id(),
                            12u,
                            new DefaultId(),
                            new DefaultEnvelope()
                        ).Id(),
                        author.Id()
                    )
                    .ToJson);
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }
    }
}