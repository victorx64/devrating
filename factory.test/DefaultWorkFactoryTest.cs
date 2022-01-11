using devrating.entity;
using devrating.factory.fake;
using Xunit;

namespace devrating.factory.test;

public sealed class DefaultWorkFactoryTest
{
    [Fact]
    public void InsertsNewAuthor()
    {
        var authors = new List<Author>();
        var entities = new FakeEntities(
            new List<Work>(),
            authors,
            new List<Rating>()
        );

        new DefaultWorkFactory(
            entities.Works(),
            entities.Ratings(),
            new DefaultAuthorFactory(entities.Authors())
        )
        .NewWork(
            "organization",
            "repository",
            "start",
            "end",
            null,
            "new author",
            null,
            DateTimeOffset.UtcNow
        );

        Assert.Single(authors);
    }

    [Fact]
    public void DoesntInsertAuthorIfExistsInOrg()
    {
        var organization = "organization";
        var repository = "repository";
        var authors = new List<Author> { new FakeAuthor(organization, repository, "existing author") };
        var entities = new FakeEntities(
            new List<Work>(),
            authors,
            new List<Rating>()
        );

        new DefaultWorkFactory(
            entities.Works(),
            entities.Ratings(),
            new DefaultAuthorFactory(entities.Authors())
        )
        .NewWork(
            organization,
            repository,
            "start",
            "end",
            null,
            "existing author",
            null,
            DateTimeOffset.UtcNow
        );

        Assert.Single(authors);
    }

    [Fact]
    public void InsertsNewAuthorIfExistsInAnotherOrg()
    {
        var repository = "repository";
        var authors = new List<Author> { new FakeAuthor("organization", repository, "existing author") };
        var entities = new FakeEntities(
            new List<Work>(),
            authors,
            new List<Rating>()
        );

        new DefaultWorkFactory(
            entities.Works(),
            entities.Ratings(),
            new DefaultAuthorFactory(entities.Authors())
        )
        .NewWork(
            "ANOTHER organization",
            repository,
            "start",
            "end",
            null,
            "existing author",
            null,
            DateTimeOffset.UtcNow
        );

        Assert.Equal(2, authors.Count);
    }

    [Fact]
    public void InsertsNewWorkWithoutUsedRating()
    {
        var works = new List<Work>();
        var entities = new FakeEntities(
            works,
            new List<Author>(),
            new List<Rating>()
        );

        new DefaultWorkFactory(
            entities.Works(),
            entities.Ratings(),
            new DefaultAuthorFactory(entities.Authors())
        )
        .NewWork(
            "organization",
            "repository",
            "start",
            "end",
            null,
            "other author",
            null,
            DateTimeOffset.UtcNow);

        Assert.False(works.Single().UsedRating().Id().Filled());
    }

    [Fact]
    public void ThrowsOnInsertingSameWorkAgain()
    {
        var entities = new FakeEntities(
            new List<Work>(),
            new List<Author>(),
            new List<Rating>()
        );

        var factory = new DefaultWorkFactory(
            entities.Works(),
            entities.Ratings(),
            new DefaultAuthorFactory(entities.Authors())
        );

        var moment1 = DateTimeOffset.UtcNow;
        var anotherMoment = moment1 + TimeSpan.FromHours(1);

        factory.NewWork(
            "organization",
            "repository",
            "start",
            "end",
            null,
            "author",
            null,
            moment1
        );

        Assert.Throws<InvalidOperationException>(() =>
        {
            factory.NewWork(
                "organization",
                "repository",
                "start",
                "end",
                null,
                "another author",
                null,
                anotherMoment
            );
        });
    }

    [Fact]
    public void ThrowsOnInsertingWorkOlderThanLatest()
    {
        var entities = new FakeEntities(
            new List<Work>(),
            new List<Author>(),
            new List<Rating>()
        );

        var factory = new DefaultWorkFactory(
            entities.Works(),
            entities.Ratings(),
            new DefaultAuthorFactory(entities.Authors())
        );

        var moment1 = DateTimeOffset.UtcNow;
        var momentBefore = moment1 - TimeSpan.FromHours(1);

        factory.NewWork(
            "organization",
            "repository",
            "start",
            "end",
            null,
            "author",
            null,
            moment1
        );

        Assert.Throws<InvalidOperationException>(() =>
        {
            factory.NewWork(
                "organization",
                "repository",
                "another start",
                "another end",
                null,
                "another author",
                null,
                momentBefore
            );
        });
    }

    [Fact]
    public void InsertsNewWorkWithUsedRating()
    {
        var organization = "organization";
        var repository = "repository";
        var author = new FakeAuthor(organization, repository, "the author");
        var work = new FakeWork(author);
        var rating = new FakeRating(1500d, work, author);
        var works = new List<Work> { work };
        var entities = new FakeEntities(
            works,
            new List<Author> { author },
            new List<Rating> { rating }
        );

        new DefaultWorkFactory(
            entities.Works(),
            entities.Ratings(),
            new DefaultAuthorFactory(entities.Authors())
        )
        .NewWork(
            organization,
            repository,
            "start",
            "end",
            null,
            author.Email(),
            null,
            DateTimeOffset.UtcNow
        );

        Assert.Equal(rating, works.Last().UsedRating());
    }
}
