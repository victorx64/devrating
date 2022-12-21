using System;
using System.Linq;
using devrating.factory;
using Microsoft.Data.Sqlite;
using Xunit;

namespace devrating.sqlite.test;

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
                        "mergeCommit",
                        null,
                        database.Entities().Authors().InsertOperation().Insert(
                            "organization",
                            "repo",
                            "email",
                            DateTimeOffset.UtcNow
                        ).Id(),
                        new DefaultId(),
                        null,
                        DateTimeOffset.UtcNow,
                        null
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
                "mergeCommit",
                null,
                database.Entities().Authors().InsertOperation().Insert(
                    "organization",
                    "repo",
                    "email",
                    DateTimeOffset.UtcNow
                ).Id(),
                new DefaultId(),
                null,
                DateTimeOffset.UtcNow,
                null
            );

            Assert.True(database.Entities().Works().ContainsOperation().Contains(
                "organization",
                "repo",
                "mergeCommit"
            ));
        }
        finally
        {
            database.Instance().Connection().Close();
        }
    }

    [Fact]
    public void ChecksIfANewerWorkExists()
    {
        var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

        database.Instance().Connection().Open();
        database.Instance().Create();

        try
        {
            var olderMoment = DateTimeOffset.UtcNow;
            var newerMoment = olderMoment + TimeSpan.FromHours(1);

            database.Entities().Works().InsertOperation().Insert(
                "mergeCommit",
                null,
                database.Entities().Authors().InsertOperation().Insert(
                    "organization",
                    "repo",
                    "email",
                    newerMoment
                ).Id(),
                new DefaultId(),
                null,
                newerMoment,
                null
            );

            Assert.True(
                database.Entities().Works().ContainsOperation().Contains(
                    "organization",
                    "repo",
                    olderMoment
                )
            );
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
                "mergeCommit",
                null,
                database.Entities().Authors().InsertOperation().Insert(
                    "organization",
                    "repo",
                    "email",
                    DateTimeOffset.UtcNow
                ).Id(),
                new DefaultId(),
                null,
                DateTimeOffset.UtcNow,
                null
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
                    "mergeCommit",
                    null,
                    database.Entities().Authors().InsertOperation().Insert(
                        "organization",
                        "repo",
                        "email",
                        DateTimeOffset.UtcNow
                    ).Id(),
                    new DefaultId(),
                    null,
                    DateTimeOffset.UtcNow,
                    null
                ).Id(),
                database.Entities().Works().GetOperation().Work(
                    "organization",
                    "repo",
                    "mergeCommit"
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
                "commit1",
                null,
                database.Entities().Authors().InsertOperation().Insert(
                    "organization",
                    "repo",
                    "author",
                    createdAt
                ).Id(),
                new DefaultId(),
                null,
                createdAt,
                null
            );

            var last = database.Entities().Works().InsertOperation().Insert(
                "commit2",
                null,
                database.Entities().Authors().InsertOperation().Insert(
                    "organization",
                    "repo",
                    "other author",
                    createdAt
                ).Id(),
                new DefaultId(),
                null,
                createdAt,
                null
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
                "commit1",
                null,
                database.Entities().Authors().InsertOperation().Insert(
                    "organization",
                    "repo",
                    "author",
                    createdAt).Id(),
                new DefaultId(),
                null,
                createdAt,
                null
            );

            database.Entities().Works().InsertOperation().Insert(
                "commit2",
                null,
                database.Entities().Authors().InsertOperation().Insert(
                    "organization",
                    "repo",
                    "other author",
                    createdAt).Id(),
                new DefaultId(),
                null,
                createdAt,
                null
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
                "commit1",
                null,
                database.Entities().Authors().InsertOperation().Insert(
                    "organization",
                    "repo",
                    "author",
                    createdInPast).Id(),
                new DefaultId(),
                null,
                createdInPast,
                null
            );

            Assert.Equal(
                database.Entities().Works().InsertOperation().Insert(
                    "commit2",
                    null,
                    database.Entities().Authors().InsertOperation().Insert(
                        "organization",
                        "repo",
                        "other author",
                        createdAt).Id(),
                    new DefaultId(),
                    null,
                    createdAt,
                    null
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
