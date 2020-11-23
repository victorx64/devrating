// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
                        new DefaultEnvelope(),
                        database.Entities().Authors().InsertOperation().Insert(
                            "organization",
                            "email",
                            DateTimeOffset.UtcNow
                        ).Id(),
                        additions,
                        new DefaultId(),
                        new DefaultEnvelope(),
                        DateTimeOffset.UtcNow
                    ).Additions()
                );
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsValidRepository()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var repository = "repo";

                Assert.Equal(
                    repository,
                    database.Entities().Works().InsertOperation().Insert(
                        repository,
                        "startCommit",
                        "endCommit",
                        new DefaultEnvelope(),
                        database.Entities().Authors().InsertOperation().Insert(
                            "organization",
                            "email",
                            DateTimeOffset.UtcNow
                        ).Id(),
                        1u,
                        new DefaultId(),
                        new DefaultEnvelope(),
                        DateTimeOffset.UtcNow
                    ).Repository()
                );
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsValidStartCommit()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var start = "startCommit";

                Assert.Equal(
                    start,
                    database.Entities().Works().InsertOperation().Insert(
                        "repo",
                        start,
                        "endCommit",
                        new DefaultEnvelope(),
                        database.Entities().Authors().InsertOperation().Insert(
                            "organization",
                            "email",
                            DateTimeOffset.UtcNow
                        ).Id(),
                        1u,
                        new DefaultId(),
                        new DefaultEnvelope(),
                        DateTimeOffset.UtcNow
                    ).Start()
                );
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsValidEndCommit()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var end = "endCommit";

                Assert.Equal(
                    end,
                    database.Entities().Works().InsertOperation().Insert(
                        "repo",
                        "startCommit",
                        end,
                        new DefaultEnvelope(),
                        database.Entities().Authors().InsertOperation().Insert(
                            "organization",
                            "email",
                            DateTimeOffset.UtcNow
                        ).Id(),
                        1u,
                        new DefaultId(),
                        new DefaultEnvelope(),
                        DateTimeOffset.UtcNow
                    ).End()
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

                Assert.Equal(
                    author.Id(),
                    database.Entities().Works().InsertOperation().Insert(
                        "repo",
                        "startCommit",
                        "endCommit",
                        new DefaultEnvelope(),
                        author.Id(),
                        2u,
                        new DefaultId(),
                        new DefaultEnvelope(),
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
        public void ReturnsValidUsedRating()
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
                    3423,
                    new DefaultEnvelope(),
                    new DefaultEnvelope(),
                    new DefaultId(),
                    database.Entities().Works().InsertOperation().Insert(
                        "repo",
                        "startCommit1",
                        "endCommit1",
                        new DefaultEnvelope(),
                        author.Id(),
                        2u,
                        new DefaultId(),
                        new DefaultEnvelope(),
                        DateTimeOffset.UtcNow
                    ).Id(),
                    author.Id(),
                    DateTimeOffset.UtcNow
                );

                Assert.Equal(
                    previous.Id(),
                    database.Entities().Works().InsertOperation().Insert(
                        "repo",
                        "startCommit",
                        "endCommit",
                        new DefaultEnvelope(),
                        author.Id(),
                        2u,
                        previous.Id(),
                        new DefaultEnvelope(),
                        DateTimeOffset.UtcNow
                    ).UsedRating().Id()
                );
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsValidSinceCommit()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var since = "sinceCommit";

                Assert.Equal(
                    since,
                    database.Entities().Works().InsertOperation().Insert(
                        "repo",
                        "startCommit",
                        "endCommit",
                        new DefaultEnvelope(since),
                        database.Entities().Authors().InsertOperation().Insert(
                            "organization",
                            "email",
                            DateTimeOffset.UtcNow
                        ).Id(),
                        1u,
                        new DefaultId(),
                        new DefaultEnvelope(),
                        DateTimeOffset.UtcNow
                    ).Since().Value()
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

                Assert.Equal(
                    moment2,
                    database.Entities().Works().InsertOperation().Insert(
                        "repo",
                        "startCommit",
                        "endCommit",
                        new DefaultEnvelope("sinceCommit"),
                        database.Entities().Authors().InsertOperation().Insert(
                            "organization",
                            "email",
                            moment1
                        )
                        .Id(),
                        1u,
                        new DefaultId(),
                        new DefaultEnvelope(),
                        moment2
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
        public void ReturnsValidLink()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var link = new DefaultEnvelope("Link");

                Assert.Equal(
                    link.Value(),
                    database.Entities().Works().InsertOperation().Insert(
                        "repo",
                        "startCommit",
                        "endCommit",
                        new DefaultEnvelope("sinceCommit"),
                        database.Entities().Authors().InsertOperation().Insert(
                            "organization",
                            "email",
                            DateTimeOffset.UtcNow
                        )
                        .Id(),
                        1u,
                        new DefaultId(),
                        link,
                        DateTimeOffset.UtcNow
                    )
                    .Link().Value()
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
                        new DefaultEnvelope(),
                        database.Entities().Authors().InsertOperation().Insert(
                            "organization",
                            "email",
                            DateTimeOffset.UtcNow
                        ).Id(),
                        2u,
                        new DefaultId(),
                        new DefaultEnvelope(),
                        DateTimeOffset.UtcNow
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