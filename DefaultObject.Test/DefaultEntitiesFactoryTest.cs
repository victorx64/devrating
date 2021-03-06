// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using DevRating.DefaultObject.Fake;
using DevRating.Domain;
using Xunit;

namespace DevRating.DefaultObject.Test
{
    public sealed class DefaultEntityFactoryTest
    {
        [Fact]
        public void InsertsNewAuthor()
        {
            var authors = new List<Author>();

            new DefaultEntityFactory(
                    new FakeEntities(
                        new List<Work>(),
                        authors,
                        new List<Rating>()
                    ),
                    new FakeFormula()
                )
                .InsertedWork(
                    "organization",
                    "repository",
                    "start",
                    "end",
                    null,
                    "new author",
                    0u,
                    null,
                    DateTimeOffset.UtcNow);

            Assert.Single(authors);
        }

        [Fact]
        public void DoesntInsertAuthorIfExistsInOrg()
        {
            var organization = "organization";
            var repository = "repository";
            var authors = new List<Author> { new FakeAuthor(organization, repository, "existing author") };

            new DefaultEntityFactory(
                    new FakeEntities(
                        new List<Work>(),
                        authors,
                        new List<Rating>()
                    ),
                    new FakeFormula()
                )
                .InsertedWork(
                    organization,
                    repository,
                    "start",
                    "end",
                    null,
                    "existing author",
                    0u,
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

            new DefaultEntityFactory(
                    new FakeEntities(
                        new List<Work>(),
                        authors,
                        new List<Rating>()
                    ),
                    new FakeFormula()
                )
                .InsertedWork(
                    "ANOTHER organization",
                    repository,
                    "start",
                    "end",
                    null,
                    "existing author",
                    0u,
                    null,
                    DateTimeOffset.UtcNow
                );

            Assert.Equal(2, authors.Count);
        }

        [Fact]
        public void InsertsNewWorkWithoutUsedRating()
        {
            var works = new List<Work>();

            new DefaultEntityFactory(
                    new FakeEntities(
                        works,
                        new List<Author>(),
                        new List<Rating>()),
                    new FakeFormula()
                )
                .InsertedWork(
                    "organization",
                    "repository",
                    "start",
                    "end",
                    null,
                    "other author",
                    0u,
                    null,
                    DateTimeOffset.UtcNow);

            Assert.False(works.Single().UsedRating().Id().Filled());
        }

        [Fact]
        public void ThrowsOnInsertingSameWorkAgain()
        {
            var factory = new DefaultEntityFactory(
                new FakeEntities(),
                new FakeFormula()
            );

            var moment1 = DateTimeOffset.UtcNow;
            var anotherMoment = moment1 + TimeSpan.FromHours(1);

            factory.InsertedWork(
                    "organization",
                    "repository",
                    "start",
                    "end",
                    null,
                    "author",
                    0u,
                    null,
                    moment1);

            Assert.Throws<InvalidOperationException>(() =>
            {
                factory.InsertedWork(
                    "organization",
                    "repository",
                    "start",
                    "end",
                    null,
                    "another author",
                    0u,
                    null,
                    anotherMoment);
            });
        }

        [Fact]
        public void ThrowsOnInsertingWorkOlderThanLatest()
        {
            var factory = new DefaultEntityFactory(
                new FakeEntities(),
                new FakeFormula()
            );

            var moment1 = DateTimeOffset.UtcNow;
            var momentBefore = moment1 - TimeSpan.FromHours(1);

            factory.InsertedWork(
                    "organization",
                    "repository",
                    "start",
                    "end",
                    null,
                    "author",
                    0u,
                    null,
                    moment1);

            Assert.Throws<InvalidOperationException>(() =>
            {
                factory.InsertedWork(
                    "organization",
                    "repository",
                    "another start",
                    "another end",
                    null,
                    "another author",
                    0u,
                    null,
                    momentBefore);
            });
        }

        [Fact]
        public void InsertsNewWorkWithUsedRating()
        {
            var organization = "organization";
            var repository = "repository";
            var author = new FakeAuthor(organization, repository, "the author");
            var work = new FakeWork(10u, author);
            var rating = new FakeRating(1500d, work, author);
            var works = new List<Work> { work };

            new DefaultEntityFactory(
                    new FakeEntities(
                        works,
                        new List<Author> { author },
                        new List<Rating> { rating }
                    ),
                    new FakeFormula()
                )
                .InsertedWork(
                    organization,
                    repository,
                    "start",
                    "end",
                    null,
                    author.Email(),
                    0u,
                    null,
                    DateTimeOffset.UtcNow);

            Assert.Equal(rating, works.Last().UsedRating());
        }

        [Fact]
        public void InsertsNewAuthorNewRatingWhenDeleteOneNewVictim()
        {
            var ratings = new List<Rating>();
            var organization = "organization";
            var repository = "repository";
            var author = new FakeAuthor(organization, repository, "author");
            var formula = new FakeFormula(10, 1);
            var deletion = new DefaultDeletion("single victim", 2);
            var newWork = new FakeWork(0u, author);

            new DefaultEntityFactory(
                    new FakeEntities(
                        new List<Work> { newWork },
                        new List<Author>(),
                        ratings
                    ),
                    formula
                )
                .InsertRatings(organization, repository, author.Email(), new[] { deletion }, newWork.Id(), DateTimeOffset.UtcNow);

            bool RatingOfAuthor(Rating r)
            {
                return r.Author().Email().Equals(author!.Email());
            }

            Assert.Equal(
                formula.WinnerNewRating(
                    formula.DefaultRating(),
                    new[]
                    {
                        new DefaultMatch(formula.DefaultRating(), deletion.Counted())
                    }
                ),
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
            var deletion = new DefaultDeletion("AUTHOR", 2);

            new DefaultEntityFactory(
                    new FakeEntities(
                        new List<Work>(),
                        new List<Author>(),
                        ratings
                    ),
                    formula
                )
                .InsertRatings(
                    organization,
                    repository,
                    author.Email(),
                    new[] { deletion },
                    new FakeWork(0u, author).Id(),
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
            var deletion = new DefaultDeletion(victim, 2);
            var newWork = new FakeWork(0u, author);

            new DefaultEntityFactory(
                    new FakeEntities(
                        new List<Work> { newWork },
                        new List<Author>(),
                        ratings
                    ),
                    formula
                )
                .InsertRatings(organization, repository, author.Email(), new[] { deletion }, newWork.Id(), DateTimeOffset.UtcNow);

            bool RatingOfVictim(Rating r)
            {
                return r.Author().Email().Equals(victim);
            }

            Assert.Equal(
                formula.LoserNewRating(
                    formula.DefaultRating(),
                    new DefaultMatch(formula.DefaultRating(), deletion.Counted())
                ),
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
            var deletion1 = new DefaultDeletion("first victim", 2);
            var deletion2 = new DefaultDeletion("second victim", 3);
            var work = new FakeWork(0u, author);

            new DefaultEntityFactory(
                    new FakeEntities(
                        new List<Work> { work },
                        new List<Author>(),
                        ratings
                    ),
                    formula
                )
                .InsertRatings(organization, repository, author.Email(), new[] { deletion1, deletion2 }, work.Id(),
                    DateTimeOffset.UtcNow);

            bool RatingOfAuthor(Rating r)
            {
                return r.Author().Email().Equals(author!.Email());
            }

            Assert.Equal(
                formula.WinnerNewRating(
                    formula.DefaultRating(),
                    new[]
                    {
                        new DefaultMatch(formula.DefaultRating(), deletion1.Counted()),
                        new DefaultMatch(formula.DefaultRating(), deletion2.Counted())
                    }
                ),
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
            var work = new FakeWork(3, victim);
            var newWork = new FakeWork(0u, author);
            var ratings = new List<Rating>
            {
                new FakeRating(50, work, victim)
            };
            var formula = new FakeFormula(10, 1);
            var deletion = new DefaultDeletion(victim.Email(), 2);

            new DefaultEntityFactory(
                    new FakeEntities(
                        new List<Work> { work, newWork },
                        new List<Author> { victim },
                        ratings
                    ),
                    formula
                )
                .InsertRatings(organization, repository, author.Email(), new[] { deletion }, newWork.Id(), DateTimeOffset.UtcNow);

            bool RatingOfAuthor(Rating r)
            {
                return r.Author().Email().Equals(author!.Email());
            }

            Assert.Equal(
                formula.WinnerNewRating(
                    formula.DefaultRating(),
                    new[]
                    {
                        new DefaultMatch(formula.DefaultRating(), deletion.Counted())
                    }
                ),
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
            var work = new FakeWork(3, victim);
            var newWork = new FakeWork(0u, author);
            var ratings = new List<Rating> { new FakeRating(50, work, victim) };
            var formula = new FakeFormula(10, 1);
            var deletion = new DefaultDeletion(victim.Email(), 2);

            new DefaultEntityFactory(
                    new FakeEntities(
                        new List<Work> { work, newWork },
                        new List<Author>(),
                        ratings
                    ),
                    formula
                )
                .InsertRatings(organization, repository, author.Email(), new[] { deletion }, newWork.Id(), DateTimeOffset.UtcNow);

            bool RatingOfVictim(Rating r)
            {
                return r.Author().Email().Equals(victim!.Email());
            }

            Assert.Equal(
                formula.LoserNewRating(
                    formula.DefaultRating(),
                    new DefaultMatch(formula.DefaultRating(), deletion.Counted())
                ),
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
            var work1 = new FakeWork(3, victim1);
            var work2 = new FakeWork(4, victim2);
            var newWork = new FakeWork(0u, author);
            var ratings = new List<Rating>
            {
                new FakeRating(50, work1, victim1),
                new FakeRating(40, work2, victim2)
            };
            var formula = new FakeFormula(10, 1);
            var deletion1 = new DefaultDeletion(victim1.Email(), 2);
            var deletion2 = new DefaultDeletion(victim2.Email(), 3);


            new DefaultEntityFactory(
                    new FakeEntities(
                        new List<Work> { work1, work2, newWork },
                        new List<Author>(),
                        ratings
                    ),
                    formula
                )
                .InsertRatings(
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
                formula.WinnerNewRating(
                    formula.DefaultRating(),
                    new[]
                    {
                        new DefaultMatch(formula.DefaultRating(), deletion1.Counted()),
                        new DefaultMatch(formula.DefaultRating(), deletion2.Counted())
                    }
                ),
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
            var work = new FakeWork(10u, author);
            var rating = new FakeRating(1500d, work, author);
            var ratings = new List<Rating> { rating };
            var formula = new FakeFormula(10, 1);
            var deletion = new DefaultDeletion(victim, 2);
            var newWork = new FakeWork(0u, author);

            new DefaultEntityFactory(
                    new FakeEntities(
                        new List<Work> { work, newWork },
                        new List<Author> { author },
                        ratings
                    ),
                    formula
                )
                .InsertRatings(
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
                formula.WinnerNewRating(
                    rating.Value(),
                    new[]
                    {
                        new DefaultMatch(formula.DefaultRating(), deletion.Counted())
                    }
                ),
                ratings.Last(RatingOfAuthor).Value()
            );
        }
    }
}