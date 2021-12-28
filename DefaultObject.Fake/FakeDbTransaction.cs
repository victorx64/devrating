// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Data;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeDbTransaction : IDbTransaction
    {
        public FakeDbTransaction(IDbConnection connection, IsolationLevel level)
        {
            Connection = connection;
            IsolationLevel = level;
        }

        public void Dispose()
        {
        }

        public void Commit()
        {
        }

        public void Rollback()
        {
        }

        public IDbConnection Connection { get; }
        public IsolationLevel IsolationLevel { get; }
    }
}