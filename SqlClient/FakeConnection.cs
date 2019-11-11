using System;
using System.Data;

namespace DevRating.SqlClient
{
    internal sealed class FakeConnection : IDbConnection
    {
        private readonly FakeCommand _command;

        public FakeConnection(FakeCommand command)
        {
            _command = command;
        }
        
        public void Dispose()
        {
        }

        public IDbTransaction BeginTransaction()
        {
            throw new NotImplementedException();
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            throw new NotImplementedException();
        }

        public void ChangeDatabase(string databaseName)
        {
        }

        public void Close()
        {
        }

        public IDbCommand CreateCommand()
        {
            return _command;
        }

        public void Open()
        {
        }

        public string ConnectionString { get; set; } = string.Empty;
        public int ConnectionTimeout { get; } = 0;
        public string Database { get; } = string.Empty;
        public ConnectionState State { get; } = ConnectionState.Open;
    }
}