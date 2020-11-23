// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
                var author = database.Entities().Authors().InsertOperation().Insert(
                    "organization",
                    "email",
                    DateTimeOffset.UtcNow
                );

                var value = 1100d;

                Assert.Equal(
                    value,
                    database.Entities().Ratings().InsertOperation().Insert(
                        value,
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
                var author = database.Entities().Authors().InsertOperation().Insert(
                    "organization",
                    "email",
                    DateTimeOffset.UtcNow
                );

                var value = 1100d;

                Assert.Equal(
                    author.Id(),
                    database.Entities().Ratings().InsertOperation().Insert(
                        value,
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
                var author = database.Entities().Authors().InsertOperation().Insert(
                    "organization",
                    "email",
                    DateTimeOffset.UtcNow
                );

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

                Assert.Equal(
                    work.Id(),
                    database.Entities().Ratings().InsertOperation().Insert(
                        1100d,
                        new DefaultEnvelope(),
                        new DefaultEnvelope(),
                        new DefaultId(),
                        work.Id(),
                        author.Id(),
                        DateTimeOffset.UtcNow
                    ).Work().Id()
                );
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsValidCountedDeletions()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var author = database.Entities().Authors().InsertOperation().Insert(
                    "organization",
                    "email",
                    DateTimeOffset.UtcNow
                );

                var deletions = new DefaultEnvelope(123123L);

                Assert.Equal(
                    deletions.Value(),
                    database.Entities().Ratings().InsertOperation().Insert(
                        1100d,
                        deletions,
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
                    ).CountedDeletions().Value()
                );
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsValidIgnoredDeletions()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var author = database.Entities().Authors().InsertOperation().Insert(
                    "organization",
                    "email",
                    DateTimeOffset.UtcNow
                );

                var deletions = new DefaultEnvelope(123123L);

                Assert.Equal(
                    deletions.Value(),
                    database.Entities().Ratings().InsertOperation().Insert(
                        1100d,
                        new DefaultEnvelope(),
                        deletions,
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
                    ).IgnoredDeletions().Value()
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
                var author = database.Entities().Authors().InsertOperation().Insert(
                    "organization",
                    "email",
                    DateTimeOffset.UtcNow
                );

                var previous = database.Entities().Ratings().InsertOperation().Insert(
                    12d,
                    new DefaultEnvelope(),
                    new DefaultEnvelope(),
                    new DefaultId(),
                    database.Entities().Works().InsertOperation().Insert(
                        "repo",
                        "startCommit1",
                        "endCommit1",
                        new DefaultEnvelope(),
                        author.Id(),
                        12u,
                        new DefaultId(),
                        new DefaultEnvelope(),
                        DateTimeOffset.UtcNow
                    ).Id(),
                    author.Id(),
                    DateTimeOffset.UtcNow
                );

                Assert.Equal(
                    previous.Id(),
                    database.Entities().Ratings().InsertOperation().Insert(
                        1100d,
                        new DefaultEnvelope(),
                        new DefaultEnvelope(),
                        previous.Id(),
                        database.Entities().Works().InsertOperation().Insert(
                            "repo",
                            "startCommit2",
                            "endCommit2",
                            new DefaultEnvelope(),
                            author.Id(),
                            1u,
                            new DefaultId(),
                            new DefaultEnvelope(),
                            DateTimeOffset.UtcNow
                        ).Id(),
                        author.Id(),
                        DateTimeOffset.UtcNow
                    ).PreviousRating().Id()
                );
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsWhenItWasCreated()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var moment1 = DateTimeOffset.UtcNow;
                var moment2 = moment1 + TimeSpan.FromDays(1);
                var moment3 = moment2 + TimeSpan.FromDays(1);

                var author = database.Entities().Authors().InsertOperation().Insert(
                    "organization",
                    "email",
                    moment1
                );

                Assert.Equal(
                    moment3,
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
                            moment2
                        )
                        .Id(),
                        author.Id(),
                        moment3
                    )
                    .CreatedAt()
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
                var author = database.Entities().Authors().InsertOperation().Insert(
                    "organization",
                    "email",
                    DateTimeOffset.UtcNow
                );

                Assert.Throws<NotImplementedException>(database.Entities().Ratings().InsertOperation().Insert(
                        12d,
                        new DefaultEnvelope(),
                        new DefaultEnvelope(),
                        new DefaultId(),
                        database.Entities().Works().InsertOperation().Insert(
                            "repo",
                            "startCommit1",
                            "endCommit1",
                            new DefaultEnvelope(),
                            author.Id(),
                            12u,
                            new DefaultId(),
                            new DefaultEnvelope(),
                            DateTimeOffset.UtcNow
                        ).Id(),
                        author.Id(),
                        DateTimeOffset.UtcNow
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