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
            "commit",
            null,
            "new author",
            null,
            DateTimeOffset.UtcNow,
            Array.Empty<string>()
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
            "commit",
            null,
            "existing author",
            null,
            DateTimeOffset.UtcNow,
            Array.Empty<string>()
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
            "commit",
            null,
            "existing author",
            null,
            DateTimeOffset.UtcNow,
            Array.Empty<string>()
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
            "commit",
            null,
            "other author",
            null,
            DateTimeOffset.UtcNow,
            Array.Empty<string>()
        );

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
            "commit",
            null,
            "author",
            null,
            moment1,
            Array.Empty<string>()
        );

        Assert.Throws<InvalidOperationException>(() =>
        {
            factory.NewWork(
                "organization",
                "repository",
                "commit",
                null,
                "another author",
                null,
                anotherMoment,
                Array.Empty<string>()
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
            "commit",
            null,
            "author",
            null,
            moment1,
            Array.Empty<string>()
        );

        Assert.Throws<InvalidOperationException>(() =>
        {
            factory.NewWork(
                "organization",
                "repository",
                "another commit",
                null,
                "another author",
                null,
                momentBefore,
                Array.Empty<string>()
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
            "commit",
            null,
            author.Email(),
            null,
            DateTimeOffset.UtcNow,
            Array.Empty<string>()
        );

        Assert.Equal(rating, works.Last().UsedRating());
    }
}
