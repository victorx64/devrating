// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Data.Sqlite;
using Xunit;

namespace DevRating.SqliteClient.Test
{
    public sealed class SqliteDatabaseInstanceTest
    {
        [Fact]
        public void DoesntHaveInstanceByDefault()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();

            try
            {
                Assert.False(database.Instance().Present());
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }

        [Fact]
        public void CreatesInstance()
        {
            var database = new SqliteDatabase(new SqliteConnection("DataSource=:memory:"));

            database.Instance().Connection().Open();

            try
            {
                database.Instance().Create();

                Assert.True(database.Instance().Present());
            }
            finally
            {
                database.Instance().Connection().Close();
            }
        }
    }
}