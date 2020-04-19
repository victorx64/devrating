// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
                var author = database.Entities().Authors().InsertOperation().Insert("organization", "email", DateTimeOffset.UtcNow);

                Assert.True(
                    database.Entities().Ratings().ContainsOperation().Contains(
                        database.Entities().Ratings().InsertOperation().Insert(
                            1100d,
                            new DefaultEnvelope(),
                            new DefaultEnvelope(),
                            new DefaultId(),
                            database.Entities().Works().InsertOperation().Insert(
                                "repo",
                                "startCommit",
                                "endCommit",
                                new DefaultEnvelope(),
                                author.Id(),
                                1u,
                                new DefaultId(),
                                new DefaultEnvelope(),
                                DateTimeOffset.UtcNow
                            ).Id(),
                            author.Id(),
                            DateTimeOffset.UtcNow
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
                var author = database.Entities().Authors().InsertOperation().Insert("organization", "email", DateTimeOffset.UtcNow);

                database.Entities().Ratings().InsertOperation().Insert(
                    1100d,
                    new DefaultEnvelope(),
                    new DefaultEnvelope(),
                    new DefaultId(),
                    database.Entities().Works().InsertOperation().Insert(
                        "repo",
                        "startCommit",
                        "endCommit",
                        new DefaultEnvelope(),
                        author.Id(),
                        1u,
                        new DefaultId(),
                        new DefaultEnvelope(),
                        DateTimeOffset.UtcNow
                    ).Id(),
                    author.Id(),
                    DateTimeOffset.UtcNow
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
                var author = database.Entities().Authors().InsertOperation().Insert("organization", "email", DateTimeOffset.UtcNow);

                var rating = database.Entities().Ratings().InsertOperation().Insert(
                    1100d,
                    new DefaultEnvelope(),
                    new DefaultEnvelope(),
                    new DefaultId(),
                    database.Entities().Works().InsertOperation().Insert(
                        "repo",
                        "startCommit",
                        "endCommit",
                        new DefaultEnvelope(),
                        author.Id(),
                        1u,
                        new DefaultId(),
                        new DefaultEnvelope(),
                        DateTimeOffset.UtcNow
                    ).Id(),
                    author.Id(),
                    DateTimeOffset.UtcNow
                );

                Assert.Equal(rating.Id(), database.Entities().Ratings().GetOperation().Rating(rating.Id()).Id());
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact(Skip = "Enable the test when the bug is fixed")] // TODO fix the bug
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
                var author = database.Entities().Authors().InsertOperation().Insert("organization", "email", DateTimeOffset.UtcNow);

                var rating = database.Entities().Ratings().InsertOperation().Insert(
                    1100d,
                    new DefaultEnvelope(),
                    new DefaultEnvelope(),
                    new DefaultId(),
                    database.Entities().Works().InsertOperation().Insert(
                        "repo",
                        "startCommit",
                        "endCommit",
                        new DefaultEnvelope(),
                        author.Id(),
                        1u,
                        new DefaultId(),
                        new DefaultEnvelope(),
                        DateTimeOffset.UtcNow
                    ).Id(),
                    author.Id(),
                    DateTimeOffset.UtcNow
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
                var author = database.Entities().Authors().InsertOperation().Insert("organization", "email", DateTimeOffset.UtcNow);

                var work = database.Entities().Works().InsertOperation().Insert(
                    "repo",
                    "startCommit",
                    "endCommit",
                    new DefaultEnvelope(),
                    author.Id(),
                    1u,
                    new DefaultId(),
                    new DefaultEnvelope(),
                    DateTimeOffset.UtcNow
                );

                var rating = database.Entities().Ratings().InsertOperation().Insert(
                    1100d,
                    new DefaultEnvelope(),
                    new DefaultEnvelope(),
                    new DefaultId(),
                    work.Id(),
                    author.Id(),
                    DateTimeOffset.UtcNow
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