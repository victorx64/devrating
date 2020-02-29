using System;
using System.Collections.Generic;
using DevRating.ConsoleApp.Fake;
using DevRating.DefaultObject;
using DevRating.DefaultObject.Fake;
using DevRating.Domain;
using DevRating.EloRating;
using Xunit;

namespace DevRating.ConsoleApp.Test
{
    public sealed class ConsoleApplicationTest
    {
        [Fact]
        public void PrintsEmptyTop()
        {
            var lines = new List<string>();

            new ConsoleApplication(new FakeDatabase(), new EloFormula()).Top(new FakeConsole(lines), "organization");

            Assert.Empty(lines);
        }

        [Fact]
        public void PrintsEveryAuthorOnCallingTop()
        {
            var organization = "organization";
            var author1 = new FakeAuthor(organization, "email1");
            var author2 = new FakeAuthor(organization, "email1");
            var authors = new List<Author> {author1, author2};
            var work = new FakeWork(1u, author1);
            var works = new List<Work> {work};
            var rating1 = new FakeRating(100, work, author1);
            var rating2 = new FakeRating(150, work, author2);
            var ratings = new List<Rating> {rating1, rating2};
            var database = new FakeDatabase(
                new FakeDbInstance(),
                new FakeEntities(
                    new FakeWorks(ratings, works, authors),
                    new FakeRatings(ratings, works, authors),
                    new FakeAuthors(authors)
                )
            );

            var lines = new List<string>();

            new ConsoleApplication(database, new EloFormula()).Top(new FakeConsole(lines), organization);

            Assert.Equal(authors.Count, lines.Count);
        }

        [Fact]
        public void SavesDiff()
        {
            var authors = new List<Author>();
            var works = new List<Work>();
            var ratings = new List<Rating>();

            new ConsoleApplication(
                new FakeDatabase(
                    new FakeDbInstance(),
                    new FakeEntities(
                        new FakeWorks(ratings, works, authors),
                        new FakeRatings(ratings, works, authors),
                        new FakeAuthors(authors)
                    )
                ), new EloFormula()
            ).Save(
                new FakeDiff(
                    "key",
                    "start",
                    "end",
                    new DefaultEnvelope(),
                    "author",
                    "org",
                    10u,
                    new[]
                    {
                        new DefaultDeletion("victim1", 7u),
                        new DefaultDeletion("victim2", 14u),
                    }
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
                "start",
                "end",
                new DefaultEnvelope(),
                "author",
                "org",
                10u,
                new[]
                {
                    new DefaultDeletion("victim1", 7u),
                    new DefaultDeletion("victim2", 14u),
                }
            );

            var application = new ConsoleApplication(
                new FakeDatabase(
                    new FakeDbInstance(),
                    new FakeEntities(
                        new FakeWorks(ratings, works, authors),
                        new FakeRatings(ratings, works, authors),
                        new FakeAuthors(authors)
                    )
                ), new EloFormula()
            );

            application.Save(diff);

            void TestCode()
            {
                application.Save(diff);
            }

            Assert.Throws<InvalidOperationException>(TestCode);
        }

        [Fact]
        public void ShowsDiff()
        {
            var authors = new List<Author>();
            var works = new List<Work>();
            var ratings = new List<Rating>();
            var lines = new List<string>();

            new ConsoleApplication(
                new FakeDatabase(
                    new FakeDbInstance(),
                    new FakeEntities(
                        new FakeWorks(ratings, works, authors),
                        new FakeRatings(ratings, works, authors),
                        new FakeAuthors(authors)
                    )
                ),
                new EloFormula()
            ).PrintTo(
                new FakeConsole(lines),
                new FakeDiff(
                    "key",
                    "start",
                    "end",
                    new DefaultEnvelope(),
                    "author",
                    "org",
                    10u,
                    new[]
                    {
                        new DefaultDeletion("victim1", 7u),
                        new DefaultDeletion("victim2", 14u),
                    }
                )
            );

            Assert.Equal(7 + ratings.Count, lines.Count);
        }
    }
}