using System.Collections.Generic;
using DevRating.Domain.Fake;
using Xunit;

namespace DevRating.Domain.Test
{
    public sealed class DefaultDiffsTest
    {
        [Fact]
        public void ReturnsDatabaseFromCtor()
        {
            var works = new List<Work>();
            var authors = new List<Author>();
            var ratings = new List<Rating>();
            var database = new FakeDatabase(
                new FakeDbInstance(
                    new FakeDbConnection(
                        new FakeDbCommand()
                    )
                ),
                new FakeEntities(
                    new FakeWorks(works, authors, ratings),
                    new FakeRatings(works, authors, ratings),
                    new FakeAuthors(authors)
                )
            );

            Assert.Equal(database, new DefaultDiffs(database, new FakeFormula()).Database());
        }

        [Fact]
        public void ReturnsFormulaFromCtor()
        {
            var formula = new FakeFormula();
            var works = new List<Work>();
            var authors = new List<Author>();
            var ratings = new List<Rating>();

            Assert.Equal(formula,
                new DefaultDiffs(
                        new FakeDatabase(
                            new FakeDbInstance(
                                new FakeDbConnection(
                                    new FakeDbCommand()
                                )
                            ),
                            new FakeEntities(
                                new FakeWorks(works, authors, ratings),
                                new FakeRatings(works, authors, ratings),
                                new FakeAuthors(authors)
                            )
                        ),
                        formula
                    )
                    .Formula()
            );
        }
    }
}