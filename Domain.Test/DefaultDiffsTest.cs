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
                    new DefaultInsertWorkParams(),
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
                    new DefaultInsertWorkParams(),
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
                    new DefaultInsertWorkParams(),
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
                    new DefaultInsertWorkParams(),
                    author.Email(),
                    new Deletion[0]);

            Assert.Equal(rating, works.Last().UsedRating());
        }

        [Fact]
        public void InsertsNewAuthorNewRatingWhenDeleteOneNewVictim()
        {
            var email = "author";
            var victim = "single victim";
            var ratings = new List<Rating>();
            var formula = new FakeFormula(10, 1);
            var deletion = new DefaultDeletion(victim, 2);

            new DefaultDiffs(
                    new FakeDatabase(
                        new FakeDbInstance(),
                        new FakeEntities(
                            new List<Work>(),
                            new List<Author>(),
                            ratings
                        )
                    ),
                    formula
                )
                .Insert(new DefaultInsertWorkParams(), email, new[] {deletion});

            bool RatingOfAuthor(Rating r)
            {
                return r.Author().Email().Equals(email);
            }

            Assert.Equal(
                formula.WinnerNewRating(
                    formula.DefaultRating(),
                    new[]
                    {
                        new DefaultMatch(formula.DefaultRating(), deletion.Count())
                    }
                ),
                ratings.Single(RatingOfAuthor).Value()
            );
        }

        [Fact]
        public void InsertsNewVictimNewRating()
        {
            var email = "author";
            var victim = "single victim";
            var ratings = new List<Rating>();
            var formula = new FakeFormula(10, 1);
            var deletion = new DefaultDeletion(victim, 2);

            new DefaultDiffs(
                    new FakeDatabase(
                        new FakeDbInstance(),
                        new FakeEntities(
                            new List<Work>(),
                            new List<Author>(),
                            ratings
                        )
                    ),
                    formula
                )
                .Insert(new DefaultInsertWorkParams(), email, new[] {deletion});

            bool RatingOfVictim(Rating r)
            {
                return r.Author().Email().Equals(victim);
            }

            Assert.Equal(
                formula.LoserNewRating(
                    formula.DefaultRating(),
                    new DefaultMatch(formula.DefaultRating(), deletion.Count())
                ),
                ratings.Single(RatingOfVictim).Value()
            );
        }

        [Fact]
        public void InsertsNewAuthorNewRatingWhenDeleteManyNewVictims()
        {
            var email = "author";
            var victim1 = "first victim";
            var victim2 = "second victim";
            var ratings = new List<Rating>();
            var formula = new FakeFormula(10, 1);
            var deletion1 = new DefaultDeletion(victim1, 2);
            var deletion2 = new DefaultDeletion(victim2, 3);

            new DefaultDiffs(
                    new FakeDatabase(
                        new FakeDbInstance(),
                        new FakeEntities(
                            new List<Work>(),
                            new List<Author>(),
                            ratings
                        )
                    ),
                    formula
                )
                .Insert(new DefaultInsertWorkParams(), email, new[] {deletion1, deletion2});

            bool RatingOfAuthor(Rating r)
            {
                return r.Author().Email().Equals(email);
            }

            Assert.Equal(
                formula.WinnerNewRating(
                    formula.DefaultRating(),
                    new[]
                    {
                        new DefaultMatch(formula.DefaultRating(), deletion1.Count()),
                        new DefaultMatch(formula.DefaultRating(), deletion2.Count())
                    }
                ),
                ratings.Single(RatingOfAuthor).Value()
            );
        }

        [Fact]
        public void InsertsNewAuthorNewRatingWhenDeleteOneOldVictim()
        {
            var email = "new author";
            var victim = new FakeAuthor("old single victim");
            var work = new FakeWork(3, victim);
            var ratings = new List<Rating> {new FakeRating(50, work, victim)};
            var formula = new FakeFormula(10, 1);
            var deletion = new DefaultDeletion(victim.Email(), 2);

            new DefaultDiffs(
                    new FakeDatabase(
                        new FakeDbInstance(),
                        new FakeEntities(
                            new List<Work> {work},
                            new List<Author> {victim},
                            ratings
                        )
                    ),
                    formula
                )
                .Insert(new DefaultInsertWorkParams(), email, new[] {deletion});

            bool RatingOfAuthor(Rating r)
            {
                return r.Author().Email().Equals(email);
            }

            Assert.Equal(
                formula.WinnerNewRating(
                    formula.DefaultRating(),
                    new[]
                    {
                        new DefaultMatch(formula.DefaultRating(), deletion.Count())
                    }
                ),
                ratings.Single(RatingOfAuthor).Value()
            );
        }

        [Fact]
        public void InsertsOldVictimNewRating()
        {
            var email = "new author";
            var victim = new FakeAuthor("old single victim");
            var work = new FakeWork(3, victim);
            var ratings = new List<Rating> {new FakeRating(50, work, victim)};
            var formula = new FakeFormula(10, 1);
            var deletion = new DefaultDeletion(victim.Email(), 2);

            new DefaultDiffs(
                    new FakeDatabase(
                        new FakeDbInstance(),
                        new FakeEntities(
                            new List<Work>(),
                            new List<Author>(),
                            ratings
                        )
                    ),
                    formula
                )
                .Insert(new DefaultInsertWorkParams(), email, new[] {deletion});

            bool RatingOfVictim(Rating r)
            {
                return r.Author().Email().Equals(victim.Email());
            }

            Assert.Equal(
                formula.LoserNewRating(
                    formula.DefaultRating(),
                    new DefaultMatch(formula.DefaultRating(), deletion.Count())
                ),
                ratings.Last(RatingOfVictim).Value()
            );
        }

        [Fact]
        public void InsertsNewAuthorNewRatingWhenDeleteManyOldVictims()
        {
            var email = "new author";
            var victim1 = new FakeAuthor("old first victim");
            var victim2 = new FakeAuthor("old second victim");
            var work1 = new FakeWork(3, victim1);
            var work2 = new FakeWork(4, victim2);
            var ratings = new List<Rating>
            {
                new FakeRating(50, work1, victim1),
                new FakeRating(40, work2, victim2)
            };
            var formula = new FakeFormula(10, 1);
            var deletion1 = new DefaultDeletion(victim1.Email(), 2);
            var deletion2 = new DefaultDeletion(victim2.Email(), 3);

            new DefaultDiffs(
                    new FakeDatabase(
                        new FakeDbInstance(),
                        new FakeEntities(
                            new List<Work>(),
                            new List<Author>(),
                            ratings
                        )
                    ),
                    formula
                )
                .Insert(new DefaultInsertWorkParams(), email, new[] {deletion1, deletion2});

            bool RatingOfAuthor(Rating r)
            {
                return r.Author().Email().Equals(email);
            }

            Assert.Equal(
                formula.WinnerNewRating(
                    formula.DefaultRating(),
                    new[]
                    {
                        new DefaultMatch(formula.DefaultRating(), deletion1.Count()),
                        new DefaultMatch(formula.DefaultRating(), deletion2.Count())
                    }
                ),
                ratings.Single(RatingOfAuthor).Value()
            );
        }

        [Fact]
        public void InsertsOldAuthorNewRatingWhenDeleteOneNewVictim()
        {
            var author = new FakeAuthor("old author");
            var victim = "single new victim";
            var work = new FakeWork(10u, author);
            var rating = new FakeRating(1500d, work, author);
            var ratings = new List<Rating> {rating};
            var formula = new FakeFormula(10, 1);
            var deletion = new DefaultDeletion(victim, 2);

            new DefaultDiffs(
                    new FakeDatabase(
                        new FakeDbInstance(),
                        new FakeEntities(
                            new List<Work> {work},
                            new List<Author> {author},
                            ratings
                        )
                    ),
                    formula
                )
                .Insert(new DefaultInsertWorkParams(), author.Email(), new[] {deletion});

            bool RatingOfAuthor(Rating r)
            {
                return r.Author().Email().Equals(author.Email());
            }

            Assert.Equal(
                formula.WinnerNewRating(
                    rating.Value(),
                    new[]
                    {
                        new DefaultMatch(formula.DefaultRating(), deletion.Count())
                    }
                ),
                ratings.Last(RatingOfAuthor).Value()
            );
        }
    }
}