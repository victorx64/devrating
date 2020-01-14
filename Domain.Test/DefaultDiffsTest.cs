using DevRating.Domain.Fake;
using Xunit;

namespace DevRating.Domain.Test
{
    public sealed class DefaultDiffsTest
    {
        [Fact]
        public void ReturnsDatabaseFromCtor()
        {
            var database = new FakeDatabase(
                new FakeDbInstance(
                    new FakeDbConnection(
                        new FakeDbCommand()
                    )
                ),
                new FakeEntities(
                    new FakeWorks(
                        new FakeInsertWorkOperation(
                            new FakeWork()
                        ),
                        new FakeGetWorkOperation(
                            new FakeWork()
                        ),
                        new FakeContainsWorkOperation()
                    ),
                    new FakeRatings(
                        new FakeInsertRatingOperation(
                            new FakeRating()
                        ),
                        new FakeGetRatingOperation(
                            new FakeRating()
                        ), new FakeContainsRatingOperation()
                    ),
                    new FakeAuthors(
                        new FakeGetAuthorOperation(
                            new FakeAuthor()
                        ),
                        new FakeInsertAuthorOperation(
                            new FakeAuthor()
                        ),
                        new FakeContainsAuthorOperation()
                    )
                )
            );

            Assert.Equal(database, new DefaultDiffs(database, new FakeFormula()).Database());
        }

        [Fact]
        public void ReturnsEntitiesFromCtor()
        {
            var formula = new FakeFormula();

            Assert.Equal(formula,
                new DefaultDiffs(
                        new FakeDatabase(
                            new FakeDbInstance(
                                new FakeDbConnection(
                                    new FakeDbCommand()
                                )
                            ),
                            new FakeEntities(
                                new FakeWorks(
                                    new FakeInsertWorkOperation(
                                        new FakeWork()
                                    ),
                                    new FakeGetWorkOperation(
                                        new FakeWork()
                                    ),
                                    new FakeContainsWorkOperation()
                                ),
                                new FakeRatings(
                                    new FakeInsertRatingOperation(
                                        new FakeRating()
                                    ),
                                    new FakeGetRatingOperation(
                                        new FakeRating()
                                    ), new FakeContainsRatingOperation()
                                ),
                                new FakeAuthors(
                                    new FakeGetAuthorOperation(
                                        new FakeAuthor()
                                    ),
                                    new FakeInsertAuthorOperation(
                                        new FakeAuthor()
                                    ),
                                    new FakeContainsAuthorOperation()
                                )
                            )
                        ),
                        formula
                    )
                    .Formula()
            );
        }
    }
}