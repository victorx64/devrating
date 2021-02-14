// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using DevRating.DefaultObject;
using Microsoft.Data.Sqlite;
using Xunit;

namespace DevRating.SqliteClient.Test
{
    public sealed class SqliteDatabaseWorksTest
    {
        [Fact]
        public void ChecksInsertedWorkById()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                Assert.True(database.Entities().Works().ContainsOperation().Contains(
                        database.Entities().Works().InsertOperation().Insert(
                            "startCommit",
                            "endCommit",
                            null,
                            database.Entities().Authors().InsertOperation().Insert(
                                "organization",
                                "repo",
                                "email",
                                DateTimeOffset.UtcNow
                            ).Id(),
                            1u,
                            new DefaultId(),
                            null,
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
        public void ChecksInsertedWorkByCommits()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                database.Entities().Works().InsertOperation().Insert(
                    "startCommit",
                    "endCommit",
                    null,
                    database.Entities().Authors().InsertOperation().Insert(
                        "organization",
                        "repo",
                        "email",
                        DateTimeOffset.UtcNow
                    ).Id(),
                    1u,
                    new DefaultId(),
                    null,
                    DateTimeOffset.UtcNow
                );

                Assert.True(database.Entities().Works().ContainsOperation().Contains(
                    "organization",
                    "repo",
                    "startCommit",
                    "endCommit"
                ));
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsInsertedWorkById()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var work = database.Entities().Works().InsertOperation().Insert(
                    "startCommit",
                    "endCommit",
                    null,
                    database.Entities().Authors().InsertOperation().Insert(
                        "organization",
                        "repo",
                        "email",
                        DateTimeOffset.UtcNow
                    ).Id(),
                    1u,
                    new DefaultId(),
                    null,
                    DateTimeOffset.UtcNow
                );

                Assert.Equal(work.Id(), database.Entities().Works().GetOperation().Work(work.Id()).Id());
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsInsertedWorkByCommits()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                Assert.Equal(
                    database.Entities().Works().InsertOperation().Insert(
                        "startCommit",
                        "endCommit",
                        null,
                        database.Entities().Authors().InsertOperation().Insert(
                            "organization",
                            "repo",
                            "email",
                            DateTimeOffset.UtcNow
                        ).Id(),
                        1u,
                        new DefaultId(),
                        null,
                        DateTimeOffset.UtcNow
                    ).Id(),
                    database.Entities().Works().GetOperation().Work(
                        "organization",
                        "repo",
                        "startCommit",
                        "endCommit"
                    ).Id()
                );
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsLastInsertedWorkFirst()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var createdAt = DateTimeOffset.UtcNow;

                database.Entities().Works().InsertOperation().Insert(
                    "start1",
                    "end1",
                    null,
                    database.Entities().Authors().InsertOperation().Insert(
                        "organization",
                        "repo",
                        "author",
                        createdAt
                    ).Id(),
                    1u,
                    new DefaultId(),
                    null,
                    createdAt
                );

                var last = database.Entities().Works().InsertOperation().Insert(
                    "start2",
                    "end2",
                    null,
                    database.Entities().Authors().InsertOperation().Insert(
                        "organization",
                        "repo",
                        "other author",
                        createdAt
                    ).Id(),
                    2u,
                    new DefaultId(),
                    null,
                    createdAt
                );

                Assert.Equal(
                    last.Id(),
                    database.Entities().Works().GetOperation().Last("organization", "repo", createdAt).First().Id()
                );
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsFirstInsertedWorkLast()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var createdAt = DateTimeOffset.UtcNow;

                var first = database.Entities().Works().InsertOperation().Insert(
                    "start1",
                    "end1",
                    null,
                    database.Entities().Authors().InsertOperation().Insert(
                        "organization",
                        "repo",
                        "author",
                        createdAt).Id(),
                    1u,
                    new DefaultId(),
                    null,
                    createdAt
                );

                database.Entities().Works().InsertOperation().Insert(
                    "start2",
                    "end2",
                    null,
                    database.Entities().Authors().InsertOperation().Insert(
                        "organization",
                        "repo",
                        "other author",
                        createdAt).Id(),
                    2u,
                    new DefaultId(),
                    null,
                    createdAt
                );

                Assert.Equal(
                    first.Id(),
                    database.Entities().Works().GetOperation().Last("organization", "repo", createdAt).Last().Id()
                );
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsLastInsertedWorkAfterTheDate()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var createdAt = DateTimeOffset.UtcNow;
                var createdInPast = createdAt - TimeSpan.FromSeconds(1);

                database.Entities().Works().InsertOperation().Insert(
                    "start1",
                    "end1",
                    null,
                    database.Entities().Authors().InsertOperation().Insert(
                        "organization",
                        "repo",
                        "author",
                        createdInPast).Id(),
                    1u,
                    new DefaultId(),
                    null,
                    createdInPast
                );

                Assert.Equal(
                    database.Entities().Works().InsertOperation().Insert(
                        "start2",
                        "end2",
                        null,
                        database.Entities().Authors().InsertOperation().Insert(
                            "organization",
                            "repo",
                            "other author",
                            createdAt).Id(),
                        2u,
                        new DefaultId(),
                        null,
                        createdAt
                    ).Id(),
                    database.Entities().Works().GetOperation().Last("organization", "repo", createdAt).Last().Id()
                );
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }
    }
}