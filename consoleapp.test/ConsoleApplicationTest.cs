using devrating.consoleapp.fake;
using devrating.entity;
using devrating.factory.fake;
using Microsoft.Extensions.Logging;
using Xunit;

namespace devrating.consoleapp.test;

public sealed class ConsoleApplicationTest
{
    [Fact]
    public void PrintsEmptyTop()
    {
        var lines = new List<string>();

        new ConsoleApplication(new LoggerFactory(), new FakeDatabase(), new FakeFormula())
            .Top(new FakeOutput(lines), "organization", "repo");

        var headers = 2;

        Assert.Equal(headers, lines.Count);
    }

    [Fact]
    public void InitsDbOnCallingTop()
    {
        var db = new FakeDatabase();

        new ConsoleApplication(new LoggerFactory(), db, new FakeFormula())
            .Top(new FakeOutput(new List<string>()), "organization", "repo");

        Assert.True(db.Instance().Present());
    }

    [Fact]
    public void InitsDbOnPrinting()
    {
        var db = new FakeDatabase();
        var diff = new FakeDiff(
            "key",
            "commit",
            null,
            "author",
            "org",
            new[]
            {
                    new FakeContemporaryLines("victim1", 7u),
                    new FakeContemporaryLines("victim2", 14u),
            },
            DateTimeOffset.UtcNow
        );

        var app = new ConsoleApplication(new LoggerFactory(), db, new FakeFormula());

        app.Save(diff);

        app.PrintTo(new FakeOutput(new List<string>()), diff);

        Assert.True(db.Instance().Present());
    }

    [Fact]
    public void ThrowsOnPrintingUnexistingDiff()
    {
        void TestCode()
        {
            new ConsoleApplication(new LoggerFactory(), new FakeDatabase(), new FakeFormula())
                .PrintTo(
                    new FakeOutput(new List<string>()),
                    new FakeDiff(
                        "key",
                        "commit",
                        null,
                        "author",
                        "org",
                        new[]
                        {
                                new FakeContemporaryLines("victim1", 7u),
                                new FakeContemporaryLines("victim2", 14u),
                        },
                        DateTimeOffset.UtcNow
                    )
                );
        }

        Assert.Throws<InvalidOperationException>(TestCode);
    }

    [Fact]
    public void PrintsEveryAuthorOnCallingTop()
    {
        var organization = "organization";
        var author1 = new FakeAuthor(organization, "repo", "email1");
        var author2 = new FakeAuthor(organization, "repo", "email1");
        var authors = new List<Author> { author1, author2 };
        var work = new FakeWork(author1);
        var works = new List<Work> { work };
        var rating1 = new FakeRating(100, work, author1);
        var rating2 = new FakeRating(150, work, author2);
        var ratings = new List<Rating> { rating1, rating2 };
        var database = new FakeDatabase(
            new FakeDbInstance(),
            new FakeEntities(
                new FakeWorks(ratings, works, authors),
                new FakeRatings(ratings, works, authors),
                new FakeAuthors(authors)
            )
        );

        var lines = new List<string>();

        new ConsoleApplication(new LoggerFactory(), database, new FakeFormula())
            .Top(new FakeOutput(lines), organization, "repo");

        var headers = 2;

        Assert.Equal(authors.Count, lines.Count - headers);
    }

    [Fact]
    public void SavesDiff()
    {
        var authors = new List<Author>();
        var works = new List<Work>();
        var ratings = new List<Rating>();

        new ConsoleApplication(new LoggerFactory(), 
            new FakeDatabase(
                new FakeDbInstance(),
                new FakeEntities(
                    new FakeWorks(ratings, works, authors),
                    new FakeRatings(ratings, works, authors),
                    new FakeAuthors(authors)
                )
            ), new FakeFormula()
        ).Save(
            new FakeDiff(
                "key",
                "commit",
                null,
                "author",
                "org",
                new[]
                {
                        new FakeContemporaryLines("victim1", 7u),
                        new FakeContemporaryLines("victim2", 14u),
                },
                DateTimeOffset.UtcNow
            )
        );

        Assert.Equal(3, authors.Count);
    }

    [Fact]
    public void ThrowsExceptionIfDiffAlreadySaved()
    {
        var authors = new List<Author>();
        var works = new List<Work>();
        var ratings = new List<Rating>();
        var diff = new FakeDiff(
            "key",
            "commit",
            null,
            "author",
            "org",
            new[]
            {
                    new FakeContemporaryLines("victim1", 7u),
                    new FakeContemporaryLines("victim2", 14u),
            },
            DateTimeOffset.UtcNow
        );

        var application = new ConsoleApplication(new LoggerFactory(), 
            new FakeDatabase(
                new FakeDbInstance(),
                new FakeEntities(
                    new FakeWorks(ratings, works, authors),
                    new FakeRatings(ratings, works, authors),
                    new FakeAuthors(authors)
                )
            ), new FakeFormula()
        );

        application.Save(diff);

        void TestCode()
        {
            application!.Save(diff!);
        }

        Assert.Throws<InvalidOperationException>(TestCode);
    }

    [Fact]
    public void ShowsDiff()
    {
        var lines = new List<string>();
        var deletions = new[]
        {
                new FakeContemporaryLines("victim1", 7u),
                new FakeContemporaryLines("victim2", 14u),
            };

        var diff = new FakeDiff(
            "key",
            "commit",
            null,
            "author",
            "org",
            deletions,
            DateTimeOffset.UtcNow
        );

        var app = new ConsoleApplication(new LoggerFactory(), new FakeDatabase(), new FakeFormula());

        app.Save(diff);

        app.PrintTo(new FakeOutput(lines), diff);

        Assert.Equal(deletions.Length, lines.Count(l => l.Contains("victim")));
    }

    [Fact]
    public void PrintsSinceCommitOfWork()
    {
        var lines = new List<string>();
        var diff = new FakeDiff();

        var app = new ConsoleApplication(new LoggerFactory(), new FakeDatabase(), new FakeFormula());

        app.Save(diff);

        app.PrintTo(new FakeOutput(lines), diff);

        Assert.Contains(lines, p => p.Equals("Since: since this commit"));
    }

    [Fact]
    public void PrintsLinkOfWork()
    {
        var lines = new List<string>();
        var diff = new FakeDiff("E.g. a link to the PR");

        var app = new ConsoleApplication(new LoggerFactory(), new FakeDatabase(), new FakeFormula());

        app.Save(diff);

        app.PrintTo(new FakeOutput(lines), diff);

        Assert.Contains(lines, p => p.Equals("Link: E.g. a link to the PR"));
    }
}
