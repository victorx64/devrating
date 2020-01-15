using System.Collections.Generic;
using System.Linq;
using DevRating.Domain.Fake;
using Xunit;

namespace DevRating.Domain.Test
{
    public sealed class DefaultDiffsTest
    {
        [Fact]
        public void ReturnsDatabaseFromCtor()
        {
            var database = new FakeDatabase();

            Assert.Equal(database, new DefaultDiffs(database, new FakeFormula()).Database());
        }

        [Fact]
        public void ReturnsFormulaFromCtor()
        {
            var formula = new FakeFormula();

            Assert.Equal(formula,
                new DefaultDiffs(
                        new FakeDatabase(),
                        formula
                    )
                    .Formula()
            );
        }

        [Fact]
        public void InsertsNewAuthor()
        {
            var authors = new List<Author>();

            new DefaultDiffs(
                    new FakeDatabase(
                        new FakeDbInstance(),
                        new FakeEntities(
                            new List<Work>(),
                            authors,
                            new List<Rating>())
                    ),
                    new FakeFormula()
                )
                .Insert(
                    new DefaultInsertWorkParams(string.Empty, string.Empty, string.Empty, 10u),
                    "new author",
                    new Deletion[0]);

            Assert.Single(authors);
        }

        [Fact]
        public void DoesntInsertAuthorIfExists()
        {
            var authors = new List<Author> {new FakeAuthor("existing author")};

            new DefaultDiffs(
                    new FakeDatabase(
                        new FakeDbInstance(),
                        new FakeEntities(
                            new List<Work>(),
                            authors,
                            new List<Rating>())
                    ),
                    new FakeFormula()
                )
                .Insert(
                    new DefaultInsertWorkParams(string.Empty, string.Empty, string.Empty, 10u),
                    "existing author",
                    new Deletion[0]);

            Assert.Single(authors);
        }

        [Fact]
        public void InsertsNewWorkWithoutUsedRating()
        {
            var works = new List<Work>();

            new DefaultDiffs(
                    new FakeDatabase(
                        new FakeDbInstance(),
                        new FakeEntities(
                            works,
                            new List<Author>(),
                            new List<Rating>())
                    ),
                    new FakeFormula()
                )
                .Insert(
                    new DefaultInsertWorkParams(string.Empty, string.Empty, string.Empty, 10u),
                    "other author",
                    new Deletion[0]);

            Assert.False(works.Single().HasUsedRating());
        }

        [Fact]
        public void InsertsNewWorkWithUsedRating()
        {
            var author = new FakeAuthor("the author");
            var work = new FakeWork(10u, author);
            var rating = new FakeRating(1500d, work, author);
            var works = new List<Work> {work};

            new DefaultDiffs(
                    new FakeDatabase(
                        new FakeDbInstance(),
                        new FakeEntities(
                            works,
                            new List<Author> {author},
                            new List<Rating> {rating}
                        )
                    ),
                    new FakeFormula()
                )
                .Insert(
                    new DefaultInsertWorkParams(string.Empty, string.Empty, string.Empty, 1u),
                    author.Email(),
                    new Deletion[0]);

            Assert.Equal(rating, works.Last().UsedRating());
        }
    }
}