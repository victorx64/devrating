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
                .Insert(string.Empty, string.Empty, string.Empty, "new author", 10u, new Deletion[0]);

            Assert.Single(authors);
        }
    }
}