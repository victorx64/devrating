using System.Data;

namespace DevRating.SqlClient
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

        public string ConnectionString
        {
            get { return _origin.ConnectionString; }
            set { _origin.ConnectionString = value; }
        }

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