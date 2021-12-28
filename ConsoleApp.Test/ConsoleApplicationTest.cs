// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
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

            new ConsoleApplication(new FakeDatabase(), new EloFormula()).Top(new FakeOutput(lines), "organization", "repo");

            var headers = 2;

            Assert.Equal(headers, lines.Count);
        }

        [Fact]
        public void InitsDbOnCallingTop()
        {
            var db = new FakeDatabase();

            new ConsoleApplication(db, new EloFormula()).Top(new FakeOutput(new List<string>()), "organization", "repo");

            Assert.True(db.Instance().Present());
        }

        [Fact]
        public void InitsDbOnPrinting()
        {
            var db = new FakeDatabase();
            var diff = new FakeDiff(
                "key",
                "start",
                "end",
                null,
                "author",
                "org",
                10u,
                new[]
                {
                    new DefaultDeletion("victim1", 7u),
                    new DefaultDeletion("victim2", 14u),
                },
                DateTimeOffset.UtcNow
            );

            var app = new ConsoleApplication(db, new EloFormula());

            app.Save(diff);

            app.PrintTo(new FakeOutput(new List<string>()), diff);

            Assert.True(db.Instance().Present());
        }

        [Fact]
        public void ThrowsOnPrintingUnexistingDiff()
        {
            void TestCode()
            {
                new ConsoleApplication(new FakeDatabase(), new EloFormula())
                    .PrintTo(
                        new FakeOutput(new List<string>()),
                        new FakeDiff(
                            "key",
                            "start",
                            "end",
                            null,
                            "author",
                            "org",
                            10u,
                            new[]
                            {
                                new DefaultDeletion("victim1", 7u),
                                new DefaultDeletion("victim2", 14u),
                            },
                            DateTimeOffset.UtcNow
                        )
                    );
            }

            Assert.Throws<InvalidOperationException>(TestCode);
        }

        [Fact]
        public void PrintsEveryAuthorOnCallingTop()
        {
            var organization = "organization";
            var author1 = new FakeAuthor(organization, "repo", "email1");
            var author2 = new FakeAuthor(organization, "repo", "email1");
            var authors = new List<Author> { author1, author2 };
            var work = new FakeWork(1u, author1);
            var works = new List<Work> { work };
            var rating1 = new FakeRating(100, work, author1);
            var rating2 = new FakeRating(150, work, author2);
            var ratings = new List<Rating> { rating1, rating2 };
            var database = new FakeDatabase(
                new FakeDbInstance(),
                new FakeEntities(
                    new FakeWorks(ratings, works, authors),
                    new FakeRatings(ratings, works, authors),
                    new FakeAuthors(authors)
                )
            );

            var lines = new List<string>();

            new ConsoleApplication(database, new EloFormula()).Top(new FakeOutput(lines), organization, "repo");

            var headers = 2;

            Assert.Equal(authors.Count, lines.Count - headers);
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
                    null,
                    "author",
                    "org",
                    10u,
                    new[]
                    {
                        new DefaultDeletion("victim1", 7u),
                        new DefaultDeletion("victim2", 14u),
                    },
                    DateTimeOffset.UtcNow
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
                null,
                "author",
                "org",
                10u,
                new[]
                {
                    new DefaultDeletion("victim1", 7u),
                    new DefaultDeletion("victim2", 14u),
                },
                DateTimeOffset.UtcNow
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
                application!.Save(diff!);
            }

            Assert.Throws<InvalidOperationException>(TestCode);
        }

        [Fact]
        public void ShowsDiff()
        {
            var lines = new List<string>();
            var deletions = new[]
            {
                new DefaultDeletion("victim1", 7u),
                new DefaultDeletion("victim2", 14u),
            };

            var diff = new FakeDiff(
                "key",
                "start",
                "end",
                null,
                "author",
                "org",
                10u,
                deletions,
                DateTimeOffset.UtcNow
            );

            var app = new ConsoleApplication(new FakeDatabase(), new EloFormula());

            app.Save(diff);

            app.PrintTo(new FakeOutput(lines), diff);

            Assert.Equal(deletions.Length, lines.Count(l => l.Contains("victim")));
        }

        [Fact]
        public void PrintsSinceCommitOfWork()
        {
            var lines = new List<string>();
            var diff = new FakeDiff();

            var app = new ConsoleApplication(new FakeDatabase(), new EloFormula());

            app.Save(diff);

            app.PrintTo(new FakeOutput(lines), diff);

            Assert.Contains(lines, p => p.Equals("Since: since this commit"));
        }

        [Fact]
        public void PrintsLinkOfWork()
        {
            var lines = new List<string>();
            var diff = new FakeDiff("E.g. a link to the PR");

            var app = new ConsoleApplication(new FakeDatabase(), new EloFormula());

            app.Save(diff);

            app.PrintTo(new FakeOutput(lines), diff);

            Assert.Contains(lines, p => p.Equals("Link: E.g. a link to the PR"));
        }
    }
}