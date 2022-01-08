using System;
using devrating.factory;
using Microsoft.Data.Sqlite;
using Xunit;

namespace devrating.sqlite.test;

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
                    new DefaultId(),
                    database.Entities().Works().InsertOperation().Insert(
                        "startCommit",
                        "endCommit",
                        null,
                        author.Id(),
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
                    new DefaultId(),
                    database.Entities().Works().InsertOperation().Insert(
                        "startCommit",
                        "endCommit",
                        null,
                        author.Id(),
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
                new DefaultId(),
                null,
                DateTimeOffset.UtcNow
            );

            Assert.Equal(
                work.Id(),
                database.Entities().Ratings().InsertOperation().Insert(
                    1100d,
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
                new DefaultId(),
                database.Entities().Works().InsertOperation().Insert(
                    "startCommit1",
                    "endCommit1",
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
                database.Entities().Ratings().InsertOperation().Insert(
                    1100d,
                    previous.Id(),
                    database.Entities().Works().InsertOperation().Insert(
                        "startCommit2",
                        "endCommit2",
                        null,
                        author.Id(),
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
}
