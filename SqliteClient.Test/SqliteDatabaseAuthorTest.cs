using System;
using Microsoft.Data.Sqlite;
using Xunit;

namespace DevRating.SqliteClient.Test
{
    public sealed class SqliteDatabaseAuthorTest
    {
        [Fact]
        public void ReturnsValidEmail()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var email = "email";

                Assert.Equal(email, database.Entities().Authors().InsertOperation().Insert(email).Email());
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
                Assert.Throws<NotImplementedException>(database.Entities().Authors().InsertOperation().Insert("email")
                    .ToJson);
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }
    }
}