// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Data;

namespace DevRating.DefaultObject
{
    public sealed class TransactedDbConnection : IDbConnection
    {
        private readonly IDbConnection _origin;
        private IDbTransaction? _transaction;

        public TransactedDbConnection(IDbConnection origin)
        {
            _origin = origin;
        }

        public void Dispose()
        {
            _origin.Dispose();
        }

        public IDbTransaction BeginTransaction()
        {
            return _transaction = _origin.BeginTransaction();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return _transaction = _origin.BeginTransaction(il);
        }

        public void ChangeDatabase(string databaseName)
        {
            _origin.ChangeDatabase(databaseName);
        }

        public void Close()
        {
            _origin.Close();
        }

        public IDbCommand CreateCommand()
        {
            var command = _origin.CreateCommand();

            command.Transaction = _transaction;

            return command;
        }

        public void Open()
        {
            _origin.Open();
        }

#nullable disable
        public string ConnectionString
        {
            get { return _origin.ConnectionString; }
            set { _origin.ConnectionString = value; }
        }
#nullable enable

        public int ConnectionTimeout
        {
            get { return _origin.ConnectionTimeout; }
        }

        public string Database
        {
            get { return _origin.Database; }
        }

        public ConnectionState State
        {
            get { return _origin.State; }
        }
    }
}
