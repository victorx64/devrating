using System;
using devrating.factory;
using Microsoft.Data.Sqlite;
using Xunit;

namespace devrating.sqlite.test;

public sealed class SqliteDatabaseWorkTest
{

    [Fact]
    public void ReturnsValidMergeCommit()
    {
        var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

        database.Instance().Connection().Open();
        database.Instance().Create();

        try
        {
            var end = "mergeCommit";

            Assert.Equal(
                end,
                database.Entities().Works().InsertOperation().Insert(
                    end,
                    null,
                    database.Entities().Authors().InsertOperation().Insert(
                        "organization",
                        "repo",
                        "email",
                        DateTimeOffset.UtcNow
                    ).Id(),
                    new DefaultId(),
                    null,
                    DateTimeOffset.UtcNow
                ).Commit()
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

            Assert.Equal(
                author.Id(),
                database.Entities().Works().InsertOperation().Insert(
                    "mergeCommit",
                    null,
                    author.Id(),
                    new DefaultId(),
                    null,
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
                "repo",
                "email",
                DateTimeOffset.UtcNow
            );

            var previous = database.Entities().Ratings().InsertOperation().Insert(
                3423,
                new DefaultId(),
                database.Entities().Works().InsertOperation().Insert(
                    "mergeCommit1",
                    null,
                    author.Id(),
                    new DefaultId(),
                    null,
                    DateTimeOffset.UtcNow
                ).Id(),
                author.Id()
            );

            Assert.Equal(
                previous.Id(),
                database.Entities().Works().InsertOperation().Insert(
                    "mergeCommit",
                    null,
                    author.Id(),
                    previous.Id(),
                    null,
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
                    "mergeCommit",
                    since,
                    database.Entities().Authors().InsertOperation().Insert(
                        "organization",
                        "repo",
                        "email",
                        DateTimeOffset.UtcNow
                    ).Id(),
                    new DefaultId(),
                    null,
                    DateTimeOffset.UtcNow
                )
                .Since()
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
                    "mergeCommit",
                    "sinceCommit",
                    database.Entities().Authors().InsertOperation().Insert(
                        "organization",
                        "repo",
                        "email",
                        moment1
                    )
                    .Id(),
                    new DefaultId(),
                    null,
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
            var link = "Link";

            Assert.Equal(
                link,
                database.Entities().Works().InsertOperation().Insert(
                    "mergeCommit",
                    "sinceCommit",
                    database.Entities().Authors().InsertOperation().Insert(
                        "organization",
                        "repo",
                        "email",
                        DateTimeOffset.UtcNow
                    )
                    .Id(),
                    new DefaultId(),
                    link,
                    DateTimeOffset.UtcNow
                )
                .Link()
            );
        }
        finally
        {
            database.Instance().Connection().Close();
        }
    }
}
