// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Data;
using DevRating.Domain;

namespace DevRating.SqliteClient
{
    public sealed class SqliteDatabase : Database
    {
        private readonly Entities _entities;
        private readonly DbInstance _instance;

        public SqliteDatabase(IDbConnection connection)
            : this(new SqliteDbInstance(connection), new SqliteEntities(connection))
        {
        }

        public SqliteDatabase(DbInstance instance, Entities entities)
        {
            _instance = instance;
            _entities = entities;
        }

        public DbInstance Instance()
        {
            return _instance;
        }

        public Entities Entities()
        {
            return _entities;
        }
    }
}