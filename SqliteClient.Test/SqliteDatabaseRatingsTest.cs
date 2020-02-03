using System;
using System.Linq;
using DevRating.DefaultObject;
using Microsoft.Data.Sqlite;
using Xunit;

namespace DevRating.SqliteClient.Test
{
    public sealed class SqliteDatabaseRatingsTest
    {
        [Fact]
        public void ChecksInsertedRatingById()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var author = database.Entities().Authors().InsertOperation().Insert("organization", "email");

                Assert.True(
                    database.Entities().Ratings().ContainsOperation().Contains(
                        database.Entities().Ratings().InsertOperation().Insert(
                            1100d,
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
                        ).Id()
                    )
                );
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ChecksInsertedRatingByAuthorId()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var author = database.Entities().Authors().InsertOperation().Insert("organization", "email");

                database.Entities().Ratings().InsertOperation().Insert(
                    1100d,
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
                );

                Assert.True(database.Entities().Ratings().ContainsOperation().ContainsRatingOf(author.Id()));
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsInsertedRatingById()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var author = database.Entities().Authors().InsertOperation().Insert("organization", "email");

                var rating = database.Entities().Ratings().InsertOperation().Insert(
                    1100d,
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
                );

                Assert.Equal(rating.Id(), database.Entities().Ratings().GetOperation().Rating(rating.Id()).Id());
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact(Skip = "Enable the test when the bug is fixed")] // "organization" fix the bug
        public void ReturnsUnfilledRatingIfNotFoundById()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                Assert.False(database.Entities().Ratings().GetOperation().Rating(new DefaultId(Guid.NewGuid())).Id()
                    .Filled());
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsInsertedRatingByAuthorId()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var author = database.Entities().Authors().InsertOperation().Insert("organization", "email");

                var rating = database.Entities().Ratings().InsertOperation().Insert(
                    1100d,
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
                );

                Assert.Equal(rating.Id(), database.Entities().Ratings().GetOperation().RatingOf(author.Id()).Id());
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsUnfilledRatingIfNotFoundByAuthorId()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                Assert.False(database.Entities().Ratings().GetOperation().RatingOf(new DefaultId(Guid.NewGuid())).Id()
                    .Filled());
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsInsertedRatingByWorkId()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var author = database.Entities().Authors().InsertOperation().Insert("organization", "email");

                var work = database.Entities().Works().InsertOperation().Insert(
                    "repo",
                    "startCommit",
                    "endCommit",
                    author.Id(),
                    1u,
                    new DefaultId(),
                    new DefaultEnvelope()
                );

                var rating = database.Entities().Ratings().InsertOperation().Insert(
                    1100d,
                    new DefaultEnvelope(),
                    new DefaultId(),
                    work.Id(),
                    author.Id()
                );

                Assert.Equal(rating.Id(),
                    database.Entities().Ratings().GetOperation().RatingsOf(work.Id()).Single().Id());
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsEmptyListIfNotFoundByWorkId()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                Assert.Empty(database.Entities().Ratings().GetOperation().RatingsOf(new DefaultId(Guid.NewGuid())));
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }
    }
}