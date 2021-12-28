// Copyright (c) 2019-present Victor Semenov
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
                    "repo",
                    "email",
                    DateTimeOffset.UtcNow
                );

                var value = 1100d;

                Assert.Equal(
                    value,
                    database.Entities().Ratings().InsertOperation().Insert(
                        value,
                        null,
                        null,
                        new DefaultId(),
                        database.Entities().Works().InsertOperation().Insert(
                            "startCommit",
                            "endCommit",
                            null,
                            author.Id(),
                            1u,
                            new DefaultId(),
                            null,
                            DateTimeOffset.UtcNow
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
                var author = database.Entities().Authors().InsertOperation().Insert(
                    "organization",
                    "repo",
                    "email",
                    DateTimeOffset.UtcNow
                );

                var value = 1100d;

                Assert.Equal(
                    author.Id(),
                    database.Entities().Ratings().InsertOperation().Insert(
                        value,
                        null,
                        null,
                        new DefaultId(),
                        database.Entities().Works().InsertOperation().Insert(
                            "startCommit",
                            "endCommit",
                            null,
                            author.Id(),
                            1u,
                            new DefaultId(),
                            null,
                            DateTimeOffset.UtcNow
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
                var author = database.Entities().Authors().InsertOperation().Insert(
                    "organization",
                    "repo",
                    "email",
                    DateTimeOffset.UtcNow
                );

                var work = database.Entities().Works().InsertOperation().Insert(
                    "startCommit",
                    "endCommit",
                    null,
                    author.Id(),
                    1u,
                    new DefaultId(),
                    null,
                    DateTimeOffset.UtcNow
                );

                Assert.Equal(
                    work.Id(),
                    database.Entities().Ratings().InsertOperation().Insert(
                        1100d,
                        null,
                        null,
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
        public void ReturnsValidCountedDeletions()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var author = database.Entities().Authors().InsertOperation().Insert(
                    "organization",
                    "repo",
                    "email",
                    DateTimeOffset.UtcNow
                );

                uint? deletions = 123u;

                Assert.Equal(
                    deletions,
                    database.Entities().Ratings().InsertOperation().Insert(
                        1100d,
                        deletions,
                        null,
                        new DefaultId(),
                        database.Entities().Works().InsertOperation().Insert(
                            "startCommit",
                            "endCommit",
                            null,
                            author.Id(),
                            1u,
                            new DefaultId(),
                            null,
                            DateTimeOffset.UtcNow
                        ).Id(),
                        author.Id()
                    )
                    .CountedDeletions()
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
                    "repo",
                    "email",
                    DateTimeOffset.UtcNow
                );

                uint? deletions = 123u;

                Assert.Equal(
                    deletions,
                    database.Entities().Ratings().InsertOperation().Insert(
                        1100d,
                        null,
                        deletions,
                        new DefaultId(),
                        database.Entities().Works().InsertOperation().Insert(
                            "startCommit",
                            "endCommit",
                            null,
                            author.Id(),
                            1u,
                            new DefaultId(),
                            null,
                            DateTimeOffset.UtcNow
                        ).Id(),
                        author.Id()
                    ).IgnoredDeletions()
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
                    "repo",
                    "email",
                    DateTimeOffset.UtcNow
                );

                var previous = database.Entities().Ratings().InsertOperation().Insert(
                    12d,
                    null,
                    null,
                    new DefaultId(),
                    database.Entities().Works().InsertOperation().Insert(
                        "startCommit1",
                        "endCommit1",
                        null,
                        author.Id(),
                        12u,
                        new DefaultId(),
                        null,
                        DateTimeOffset.UtcNow
                    ).Id(),
                    author.Id()
                );

                Assert.Equal(
                    previous.Id(),
                    database.Entities().Ratings().InsertOperation().Insert(
                        1100d,
                        null,
                        null,
                        previous.Id(),
                        database.Entities().Works().InsertOperation().Insert(
                            "startCommit2",
                            "endCommit2",
                            null,
                            author.Id(),
                            1u,
                            new DefaultId(),
                            null,
                            DateTimeOffset.UtcNow
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
                var author = database.Entities().Authors().InsertOperation().Insert(
                    "organization",
                    "repo",
                    "email",
                    DateTimeOffset.UtcNow
                );

                Assert.Throws<NotImplementedException>(database.Entities().Ratings().InsertOperation().Insert(
                        12d,
                        null,
                        null,
                        new DefaultId(),
                        database.Entities().Works().InsertOperation().Insert(
                            "startCommit1",
                            "endCommit1",
                            null,
                            author.Id(),
                            12u,
                            new DefaultId(),
                            null,
                            DateTimeOffset.UtcNow
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