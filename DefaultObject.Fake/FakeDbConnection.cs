// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Data;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeDbConnection : IDbConnection
    {
        private readonly IDbCommand _command;

        public FakeDbConnection() : this(new FakeDbCommand())
        {
        }

        public FakeDbConnection(IDbCommand command)
        {
            _command = command;
        }

        public void Dispose()
        {
            Close();
        }

        public IDbTransaction BeginTransaction()
        {
            return new FakeDbTransaction(this, IsolationLevel.Unspecified);
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return new FakeDbTransaction(this, il);
        }

        public void ChangeDatabase(string databaseName)
        {
            Database = databaseName;
        }

        public void Close()
        {
            State = ConnectionState.Closed;
        }

        public IDbCommand CreateCommand()
        {
            return _command;
        }

        public void Open()
        {
            State = ConnectionState.Open;
        }

#nullable disable
        public string ConnectionString { get; set; } = string.Empty;
#nullable enable
        public int ConnectionTimeout { get; } = 10;
        public string Database { get; private set; } = string.Empty;
        public ConnectionState State { get; private set; }
    }
}