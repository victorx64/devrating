// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using Microsoft.Data.Sqlite;
using Xunit;

namespace DevRating.SqliteClient.Test
{
    public sealed class SqliteDatabaseAuthorTest
    {
        [Fact]
        public void ReturnsValidOrganization()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var organization = "organization";

                Assert.Equal(organization,
                    database.Entities().Authors().InsertOperation().Insert(organization, "email", DateTimeOffset.UtcNow).Organization());
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsValidEmail()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var email = "email";

                Assert.Equal(email,
                    database.Entities().Authors().InsertOperation().Insert("organization", email, DateTimeOffset.UtcNow).Email());
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsWhenItWasCreated()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var moment1 = DateTimeOffset.UtcNow;

                Assert.Equal(
                    moment1,
                    database.Entities().Authors().InsertOperation().Insert("organization", "email", moment1).CreatedAt()
                );
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void DoesntImplementToJson()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                Assert.Throws<NotImplementedException>(database.Entities().Authors().InsertOperation()
                    .Insert("organization", "email", DateTimeOffset.UtcNow)
                    .ToJson);
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }
    }
}