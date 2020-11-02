// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using DevRating.DefaultObject;
using Microsoft.Data.Sqlite;
using Xunit;

namespace DevRating.SqliteClient.Test
{
    public sealed class SqliteDatabaseAuthorsTest
    {
        [Fact]
        public void ChecksInsertedAuthorById()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                Assert.True(database.Entities().Authors().ContainsOperation()
                    .Contains(database.Entities().Authors().InsertOperation()
                        .Insert("organization", "email", DateTimeOffset.UtcNow).Id()));
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ChecksInsertedAuthorByOrgAndEmail()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var organization = "organization";

                Assert.True(database.Entities().Authors().ContainsOperation().Contains(organization,
                    database.Entities().Authors().InsertOperation().Insert(organization, "email", DateTimeOffset.UtcNow)
                        .Email()));
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void InsertsLongEmail()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var organization = "organization";
                var email = "longer.than.50.longer.than.50.longer.than.50.longer.than.50.longer.than.50";

                Assert.Equal(
                    email,
                    database.Entities().Authors().InsertOperation().Insert(organization, email, DateTimeOffset.UtcNow).Email()
                );
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsInsertedAuthorByOrgAndEmail()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var organization = "organization";

                var author = database.Entities().Authors().InsertOperation()
                    .Insert(organization, "email", DateTimeOffset.UtcNow);

                Assert.Equal(author.Id(),
                    database.Entities().Authors().GetOperation().Author(organization, author.Email()).Id());
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsInsertedAuthorById()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var author = database.Entities().Authors().InsertOperation()
                    .Insert("organization", "email", DateTimeOffset.UtcNow);

                Assert.Equal(author.Id(), database.Entities().Authors().GetOperation().Author(author.Id()).Id());
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsOrganizationTopAuthors()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var organization = "organization";

                var author1 = database.Entities().Authors().InsertOperation()
                    .Insert(organization, "email1", DateTimeOffset.UtcNow);
                var author2 = database.Entities().Authors().InsertOperation()
                    .Insert(organization, "email2", DateTimeOffset.UtcNow);
                var author3 = database.Entities().Authors().InsertOperation()
                    .Insert("ANOTHER organization", "email3", DateTimeOffset.UtcNow);

                var work1 = database.Entities().Works().InsertOperation().Insert(
                    "repo",
                    "start",
                    "end",
                    new DefaultEnvelope(),
                    author1.Id(),
                    1u,
                    new DefaultId(),
                    new DefaultEnvelope(),
                    DateTimeOffset.UtcNow
                );

                database.Entities().Ratings().InsertOperation().Insert(
                    100,
                    new DefaultEnvelope(),
                    new DefaultEnvelope(),
                    new DefaultId(),
                    work1.Id(),
                    author1.Id(),
                    DateTimeOffset.UtcNow
                );

                database.Entities().Ratings().InsertOperation().Insert(
                    50,
                    new DefaultEnvelope(),
                    new DefaultEnvelope(),
                    new DefaultId(),
                    work1.Id(),
                    author2.Id(),
                    DateTimeOffset.UtcNow
                );

                var work2 = database.Entities().Works().InsertOperation().Insert(
                    "other repo",
                    "other start",
                    "other end",
                    new DefaultEnvelope(),
                    author3.Id(),
                    1u,
                    new DefaultId(),
                    new DefaultEnvelope(),
                    DateTimeOffset.UtcNow
                );

                database.Entities().Ratings().InsertOperation().Insert(
                    150,
                    new DefaultEnvelope(),
                    new DefaultEnvelope(),
                    new DefaultId(),
                    work2.Id(),
                    author3.Id(),
                    DateTimeOffset.UtcNow
                );

                Assert.Equal(author1.Id(),
                    database.Entities().Authors().GetOperation().TopOfOrganization(organization).First().Id());
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsRepoTopAuthors()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var organization = "organization";
                var author1 = database.Entities().Authors().InsertOperation()
                    .Insert(organization, "email1", DateTimeOffset.UtcNow);
                var author2 = database.Entities().Authors().InsertOperation()
                    .Insert(organization, "email2", DateTimeOffset.UtcNow);
                var author3 = database.Entities().Authors().InsertOperation()
                    .Insert(organization, "email3", DateTimeOffset.UtcNow);

                var repository = "first repo";

                var work1 = database.Entities().Works().InsertOperation().Insert(
                    repository,
                    "start",
                    "end",
                    new DefaultEnvelope(),
                    author1.Id(),
                    1u,
                    new DefaultId(),
                    new DefaultEnvelope(),
                    DateTimeOffset.UtcNow
                );

                database.Entities().Ratings().InsertOperation().Insert(
                    100,
                    new DefaultEnvelope(),
                    new DefaultEnvelope(),
                    new DefaultId(),
                    work1.Id(),
                    author1.Id(),
                    DateTimeOffset.UtcNow
                );

                database.Entities().Ratings().InsertOperation().Insert(
                    50,
                    new DefaultEnvelope(),
                    new DefaultEnvelope(),
                    new DefaultId(),
                    work1.Id(),
                    author2.Id(),
                    DateTimeOffset.UtcNow
                );

                var work2 = database.Entities().Works().InsertOperation().Insert(
                    "OTHER REPOSITORY",
                    "some start commit",
                    "some end commit",
                    new DefaultEnvelope(),
                    author1.Id(),
                    1u,
                    new DefaultId(),
                    new DefaultEnvelope(),
                    DateTimeOffset.UtcNow
                );

                database.Entities().Ratings().InsertOperation().Insert(
                    75,
                    new DefaultEnvelope(),
                    new DefaultEnvelope(),
                    new DefaultId(),
                    work2.Id(),
                    author3.Id(),
                    DateTimeOffset.UtcNow
                );

                Assert.Equal(2, database.Entities().Authors().GetOperation().TopOfRepository(repository).Count());
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }
    }
}