using System;
using System.Linq;
using devrating.factory;
using Microsoft.Data.Sqlite;
using Xunit;

namespace devrating.sqlite.test;

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
            var author = database.Entities().Authors().InsertOperation()
                .Insert("organization", "repo", "email", DateTimeOffset.UtcNow);

            Assert.True(
                database.Entities().Ratings().ContainsOperation().Contains(
                    database.Entities().Ratings().InsertOperation().Insert(
                        1100d,
                        new DefaultId(),
                        database.Entities().Works().InsertOperation().Insert(
                            "mergeCommit",
                            null,
                            author.Id(),
                            new DefaultId(),
                            null,
                            DateTimeOffset.UtcNow,
                            null
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
            var author = database.Entities().Authors().InsertOperation()
                .Insert("organization", "repo", "email", DateTimeOffset.UtcNow);

            database.Entities().Ratings().InsertOperation().Insert(
                1100d,
                new DefaultId(),
                database.Entities().Works().InsertOperation().Insert(
                    "mergeCommit",
                    null,
                    author.Id(),
                    new DefaultId(),
                    null,
                    DateTimeOffset.UtcNow,
                    null
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
            var author = database.Entities().Authors().InsertOperation()
                .Insert("organization", "repo", "email", DateTimeOffset.UtcNow);

            var rating = database.Entities().Ratings().InsertOperation().Insert(
                1100d,
                new DefaultId(),
                database.Entities().Works().InsertOperation().Insert(
                    "mergeCommit",
                    null,
                    author.Id(),
                    new DefaultId(),
                    null,
                    DateTimeOffset.UtcNow,
                    null
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

    [Fact]
    public void ReturnsInsertedRatingByAuthorId()
    {
        var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

        database.Instance().Connection().Open();
        database.Instance().Create();

        try
        {
            var author = database.Entities().Authors().InsertOperation()
                .Insert("organization", "repo", "email", DateTimeOffset.UtcNow);

            var rating = database.Entities().Ratings().InsertOperation().Insert(
                1100d,
                new DefaultId(),
                database.Entities().Works().InsertOperation().Insert(
                    "mergeCommit",
                    null,
                    author.Id(),
                    new DefaultId(),
                    null,
                    DateTimeOffset.UtcNow,
                    null
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
            var author = database.Entities().Authors().InsertOperation()
                .Insert("organization", "repo", "email", DateTimeOffset.UtcNow);

            var work = database.Entities().Works().InsertOperation().Insert(
                "mergeCommit",
                null,
                author.Id(),
                new DefaultId(),
                null,
                DateTimeOffset.UtcNow,
                null
            );

            var rating = database.Entities().Ratings().InsertOperation().Insert(
                1100d,
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

    [Fact]
    public void ReturnsLastInsertedRatingsOfAuthor()
    {
        var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

        database.Instance().Connection().Open();
        database.Instance().Create();

        try
        {
            var author = database.Entities().Authors().InsertOperation()
                .Insert("organization", "repo", "email", DateTimeOffset.UtcNow);

            var date = new DateTimeOffset(2020, 1, 1, 0, 0, 0, TimeSpan.Zero);

            var work1 = database.Entities().Works().InsertOperation().Insert(
                "mergeCommit1",
                null,
                author.Id(),
                new DefaultId(),
                null,
                date,
                null
            );

            var rating1 = database.Entities().Ratings().InsertOperation().Insert(
                1100d,
                new DefaultId(),
                work1.Id(),
                author.Id()
            );

            var work2 = database.Entities().Works().InsertOperation().Insert(
                "mergeCommit2",
                null,
                author.Id(),
                new DefaultId(),
                null,
                work1.CreatedAt() + TimeSpan.FromHours(0.5),
                null
            );

            var rating2 = database.Entities().Ratings().InsertOperation().Insert(
                1200d,
                new DefaultId(),
                work2.Id(),
                author.Id()
            );

            Assert.Equal(
                rating2.Id(),
                database.Entities().Ratings().GetOperation().Last(
                        author.Id(),
                        work2.CreatedAt()
                    )
                    .Single()
                    .Id()
            );
        }
        finally
        {
            database.Instance().Connection().Close();
        }
    }
}
