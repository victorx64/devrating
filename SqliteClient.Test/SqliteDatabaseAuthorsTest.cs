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
                    .Contains(database.Entities().Authors().InsertOperation().Insert("email").Id()));
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ChecksInsertedAuthorByEmail()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                Assert.True(database.Entities().Authors().ContainsOperation()
                    .Contains(database.Entities().Authors().InsertOperation().Insert("email").Email()));
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void ReturnsInsertedAuthorByEmail()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();
            database.Instance().Create();

            try
            {
                var author = database.Entities().Authors().InsertOperation().Insert("email");

                Assert.Equal(author.Id(), database.Entities().Authors().GetOperation().Author(author.Email()).Id());
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
                var author = database.Entities().Authors().InsertOperation().Insert("email");

                Assert.Equal(author.Id(), database.Entities().Authors().GetOperation().Author(author.Id()).Id());
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }
    }
}