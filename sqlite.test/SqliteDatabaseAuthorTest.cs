using System;
using Microsoft.Data.Sqlite;
using Xunit;

namespace devrating.sqlite.test;

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

            Assert.Equal(
                organization,
                database
                    .Entities()
                    .Authors()
                    .InsertOperation()
                    .Insert(organization, "repo", "email", DateTimeOffset.UtcNow)
                    .Organization()
            );
        }
        finally
        {
            database.Instance().Connection().Close();
        }
    }

    [Fact]
    public void ReturnsValidRepository()
    {
        var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

        database.Instance().Connection().Open();
        database.Instance().Create();

        try
        {
            var repo = "repo";

            Assert.Equal(
                repo,
                database
                    .Entities()
                    .Authors()
                    .InsertOperation()
                    .Insert("organization", repo, "email", DateTimeOffset.UtcNow)
                    .Repository()
            );
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
                database.Entities().Authors().InsertOperation().Insert("organization", "repo", email, DateTimeOffset.UtcNow).Email());
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
                database.Entities().Authors().InsertOperation().Insert("organization", "repo", "email", moment1).CreatedAt()
            );
        }
        finally
        {
            database.Instance().Connection().Close();
        }
    }
}
