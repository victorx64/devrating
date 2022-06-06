using devrating.entity;
using devrating.factory.fake;
using Microsoft.Extensions.Logging;
using Xunit;

namespace devrating.factory.test;

public sealed class DefaultRatingFactoryTest
{
    [Fact]
    public void InsertsNewAuthorNewRatingWhenDeleteOneNewVictim()
    {
        var ratings = new List<Rating>();
        var organization = "organization";
        var repository = "repository";
        var author = new FakeAuthor(organization, repository, "author");
        var formula = new FakeFormula(10, 1);
        var deletion = new FakeContemporaryLines("single victim", 2);
        var newWork = new FakeWork(author);
        var entities = new FakeEntities(
            new List<Work> { newWork },
            new List<Author>(),
            ratings
        );

        new DefaultRatingFactory(
            new LoggerFactory(),
            new DefaultAuthorFactory(entities.Authors()),
            entities.Ratings(),
            formula
        )
        .NewRatings(
            organization,
            repository,
            author.Email(),
            new[] { deletion },
            newWork.Id(),
            DateTimeOffset.UtcNow
        );

        bool RatingOfAuthor(Rating r)
        {
            return r.Author().Email().Equals(author!.Email());
        }

        Assert.Equal(
            formula.DefaultRating() + formula.WinnerRatingChange(0, 0),
            ratings.Single(RatingOfAuthor).Value()
        );
    }

    [Fact]
    public void DoesntInsertsNewAuthorNewRatingWhenHeDeletedHimself()
    {
        var ratings = new List<Rating>();
        var organization = "organization";
        var repository = "repository";
        var author = new FakeAuthor(organization, repository, "author");
        var formula = new FakeFormula(10, 1);
        var deletion = new FakeContemporaryLines("AUTHOR", 2);
        var entities = new FakeEntities(
            new List<Work>(),
            new List<Author>(),
            ratings
        );

        new DefaultRatingFactory(
            new LoggerFactory(),
            new DefaultAuthorFactory(entities.Authors()),
            entities.Ratings(),
            formula
        )
        .NewRatings(
            organization,
            repository,
            author.Email(),
            new[] { deletion },
            new FakeWork(author).Id(),
            DateTimeOffset.UtcNow
        );

        Assert.Empty(ratings);
    }

    [Fact]
    public void InsertsNewVictimNewRating()
    {
        var ratings = new List<Rating>();
        var organization = "organization";
        var repository = "repository";
        var author = new FakeAuthor(organization, repository, "author");
        var formula = new FakeFormula(10, 1);
        var victim = "single victim";
        var deletion = new FakeContemporaryLines(victim, 2);
        var newWork = new FakeWork(author);

        var entities = new FakeEntities(
            new List<Work> { newWork },
            new List<Author>(),
            ratings
        );

        new DefaultRatingFactory(
            new LoggerFactory(),
            new DefaultAuthorFactory(entities.Authors()),
            entities.Ratings(),
            formula
        )
        .NewRatings(
            organization,
            repository,
            author.Email(),
            new[] { deletion },
            newWork.Id(),
            DateTimeOffset.UtcNow
        );

        bool RatingOfVictim(Rating r)
        {
            return r.Author().Email().Equals(victim);
        }

        Assert.Equal(
            formula.DefaultRating() - formula.WinnerRatingChange(0, 0),
            ratings.Single(RatingOfVictim).Value()
        );
    }

    [Fact]
    public void InsertsNewAuthorNewRatingWhenDeleteManyNewVictims()
    {
        var organization = "organization";
        var repository = "repository";
        var author = new FakeAuthor(organization, repository, "author");
        var ratings = new List<Rating>();
        var formula = new FakeFormula(10, 1);
        var deletion1 = new FakeContemporaryLines("first victim", 2);
        var deletion2 = new FakeContemporaryLines("second victim", 3);
        var work = new FakeWork(author);
        var entities = new FakeEntities(
            new List<Work> { work },
            new List<Author>(),
            ratings
        );

        new DefaultRatingFactory(
            new LoggerFactory(),
            new DefaultAuthorFactory(entities.Authors()),
            entities.Ratings(),
            formula
        )
        .NewRatings(
            organization,
            repository,
            author.Email(),
            new[] { deletion1, deletion2 },
            work.Id(),
            DateTimeOffset.UtcNow
        );

        bool RatingOfAuthor(Rating r)
        {
            return r.Author().Email().Equals(author!.Email());
        }

        Assert.Equal(
            formula.DefaultRating() + formula.WinnerRatingChange(0, 0) * 2,
            ratings.Single(RatingOfAuthor).Value()
        );
    }

    [Fact]
    public void InsertsNewAuthorNewRatingWhenDeleteOneOldVictim()
    {
        var organization = "organization";
        var repository = "repository";
        var author = new FakeAuthor(organization, repository, "new author");
        var victim = new FakeAuthor(organization, repository, "old single victim");
        var work = new FakeWork(victim);
        var newWork = new FakeWork(author);
        var ratings = new List<Rating>
        {
            new FakeRating(50, work, victim)
        };
        var formula = new FakeFormula(10, 1);
        var deletion = new FakeContemporaryLines(victim.Email(), 2);
        var entities = new FakeEntities(
            new List<Work> { work, newWork },
            new List<Author> { victim },
            ratings
        );

        new DefaultRatingFactory(
            new LoggerFactory(),
            new DefaultAuthorFactory(entities.Authors()),
            entities.Ratings(),
            formula
        )
        .NewRatings(
            organization,
            repository,
            author.Email(),
            new[] { deletion },
            newWork.Id(),
            DateTimeOffset.UtcNow
        );

        bool RatingOfAuthor(Rating r)
        {
            return r.Author().Email().Equals(author!.Email());
        }

        Assert.Equal(
            formula.DefaultRating() + formula.WinnerRatingChange(0, 0) * 1,
            ratings.Single(RatingOfAuthor).Value()
        );
    }

    [Fact]
    public void InsertsOldVictimNewRating()
    {
        var organization = "organization";
        var repository = "repository";
        var author = new FakeAuthor(organization, repository, "new author");
        var victim = new FakeAuthor(organization, repository, "old single victim");
        var work = new FakeWork(victim);
        var newWork = new FakeWork(author);
        var ratings = new List<Rating> { new FakeRating(50, work, victim) };
        var formula = new FakeFormula(10, 1);
        var deletion = new FakeContemporaryLines(victim.Email(), 2);
        var entities = new FakeEntities(
            new List<Work> { work, newWork },
            new List<Author>(),
            ratings
        );

        new DefaultRatingFactory(
            new LoggerFactory(),
            new DefaultAuthorFactory(entities.Authors()),
            entities.Ratings(),
            formula
        )
        .NewRatings(
            organization,
            repository,
            author.Email(),
            new[] { deletion },
            newWork.Id(),
            DateTimeOffset.UtcNow
        );

        bool RatingOfVictim(Rating r)
        {
            return r.Author().Email().Equals(victim!.Email());
        }

        Assert.Equal(
            formula.DefaultRating() - formula.WinnerRatingChange(0, 0) * 1,
            ratings.Last(RatingOfVictim).Value()
        );
    }

    [Fact]
    public void InsertsNewAuthorNewRatingWhenDeleteManyOldVictims()
    {
        var organization = "organization";
        var repository = "repository";
        var author = new FakeAuthor(organization, repository, "new author");
        var victim1 = new FakeAuthor(organization, repository, "old first victim");
        var victim2 = new FakeAuthor(organization, repository, "old second victim");
        var work1 = new FakeWork(victim1);
        var work2 = new FakeWork(victim2);
        var newWork = new FakeWork(author);
        var ratings = new List<Rating>
        {
            new FakeRating(50, work1, victim1),
            new FakeRating(40, work2, victim2)
        };
        var formula = new FakeFormula(10, 1);
        var deletion1 = new FakeContemporaryLines(victim1.Email(), 2);
        var deletion2 = new FakeContemporaryLines(victim2.Email(), 3);
        var entities = new FakeEntities(
            new List<Work> { work1, work2, newWork },
            new List<Author>(),
            ratings
        );

        new DefaultRatingFactory(
            new LoggerFactory(),
            new DefaultAuthorFactory(entities.Authors()),
            entities.Ratings(),
            formula
        )
        .NewRatings(
            organization,
            repository,
            author.Email(),
            new[] { deletion1, deletion2 },
            newWork.Id(),
            DateTimeOffset.UtcNow
        );

        bool RatingOfAuthor(Rating r)
        {
            return r.Author().Email().Equals(author!.Email());
        }

        Assert.Equal(
            formula.DefaultRating() + formula.WinnerRatingChange(0, 0) * 2,
            ratings.Single(RatingOfAuthor).Value()
        );
    }

    [Fact]
    public void InsertsOldAuthorNewRatingWhenDeleteOneNewVictim()
    {
        var organization = "organization";
        var repository = "repository";
        var author = new FakeAuthor(organization, repository, "old author");
        var victim = "single new victim";
        var work = new FakeWork(author);
        var rating = new FakeRating(1500d, work, author);
        var ratings = new List<Rating> { rating };
        var formula = new FakeFormula(10, 1);
        var deletion = new FakeContemporaryLines(victim, 2);
        var newWork = new FakeWork(author);
        var entities = new FakeEntities(
            new List<Work> { work, newWork },
            new List<Author> { author },
            ratings
        );

        new DefaultRatingFactory(
            new LoggerFactory(),
            new DefaultAuthorFactory(entities.Authors()),
            entities.Ratings(),
            formula
        )
        .NewRatings(
            organization,
            repository,
            author.Email(),
            new[] { deletion },
            newWork.Id(),
            DateTimeOffset.UtcNow
        );

        bool RatingOfAuthor(Rating r)
        {
            return r.Author().Email().Equals(author!.Email());
        }
        
        Assert.Equal(
            rating.Value() + formula.WinnerRatingChange(0, 0) * 1,
            ratings.Last(RatingOfAuthor).Value()
        );
    }
}
