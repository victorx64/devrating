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

                var moment = DateTimeOffset.UtcNow;

                var author1 = database.Entities().Authors().InsertOperation()
                    .Insert(organization, "email1", moment);
                var author2 = database.Entities().Authors().InsertOperation()
                    .Insert(organization, "email2", moment);
                var author3 = database.Entities().Authors().InsertOperation()
                    .Insert("ANOTHER organization", "email3", moment);

                var work1 = database.Entities().Works().InsertOperation().Insert(
                    "repo",
                    "start",
                    "end",
                    null,
                    author1.Id(),
                    1u,
                    new DefaultId(),
                    null,
                    moment
                );

                database.Entities().Ratings().InsertOperation().Insert(
                    100,
                    null,
                    null,
                    new DefaultId(),
                    work1.Id(),
                    author1.Id()
                );

                database.Entities().Ratings().InsertOperation().Insert(
                    50,
                    null,
                    null,
                    new DefaultId(),
                    work1.Id(),
                    author2.Id()
                );

                var work2 = database.Entities().Works().InsertOperation().Insert(
                    "other repo",
                    "other start",
                    "other end",
                    null,
                    author3.Id(),
                    1u,
                    new DefaultId(),
                    null,
                    moment
                );

                database.Entities().Ratings().InsertOperation().Insert(
                    150,
                    null,
                    null,
                    new DefaultId(),
                    work2.Id(),
                    author3.Id()
                );

                Assert.Equal(author1.Id(),
                    database.Entities().Authors().GetOperation()
                    .TopOfOrganization(organization, moment - TimeSpan.FromDays(1)).First().Id());
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
                var moment = DateTimeOffset.UtcNow;

                var work1 = database.Entities().Works().InsertOperation().Insert(
                    repository,
                    "start",
                    "end",
                    null,
                    author1.Id(),
                    1u,
                    new DefaultId(),
                    null,
                    moment
                );

                database.Entities().Ratings().InsertOperation().Insert(
                    100,
                    null,
                    null,
                    new DefaultId(),
                    work1.Id(),
                    author1.Id()
                );

                database.Entities().Ratings().InsertOperation().Insert(
                    50,
                    null,
                    null,
                    new DefaultId(),
                    work1.Id(),
                    author2.Id()
                );

                var work2 = database.Entities().Works().InsertOperation().Insert(
                    "OTHER REPOSITORY",
                    "some start commit",
                    "some end commit",
                    null,
                    author1.Id(),
                    1u,
                    new DefaultId(),
                    null,
                    moment
                );

                database.Entities().Ratings().InsertOperation().Insert(
                    75,
                    null,
                    null,
                    new DefaultId(),
                    work2.Id(),
                    author3.Id()
                );

                Assert.Equal(2, database.Entities().Authors().GetOperation()
                .TopOfRepository(repository, moment - TimeSpan.FromDays(1)).Count());
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }
    }
}