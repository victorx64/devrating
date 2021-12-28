// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DevRating.Domain;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeDatabase : Database
    {
        private readonly DbInstance _instance;
        private readonly Entities _entities;

        public FakeDatabase() : this(new FakeDbInstance(), new FakeEntities())
        {
        }

        public FakeDatabase(DbInstance instance, Entities entities)
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