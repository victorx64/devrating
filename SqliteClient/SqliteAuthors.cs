// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Data;
using DevRating.Domain;

namespace DevRating.SqliteClient
{
    internal sealed class SqliteAuthors : Authors
    {
        private readonly GetAuthorOperation _get;
        private readonly InsertAuthorOperation _insert;
        private readonly ContainsAuthorOperation _contains;

        public SqliteAuthors(IDbConnection connection)
            : this(new SqliteGetAuthorOperation(connection),
                new SqliteInsertAuthorOperation(connection),
                new SqliteContainsAuthorOperation(connection))
        {
        }

        public SqliteAuthors(GetAuthorOperation get, InsertAuthorOperation insert, ContainsAuthorOperation contains)
        {
            _get = get;
            _insert = insert;
            _contains = contains;
        }

        public GetAuthorOperation GetOperation()
        {
            return _get;
        }

        public InsertAuthorOperation InsertOperation()
        {
            return _insert;
        }

        public ContainsAuthorOperation ContainsOperation()
        {
            return _contains;
        }
    }
}