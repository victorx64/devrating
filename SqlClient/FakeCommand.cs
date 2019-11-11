using System.Data;
using Microsoft.Data.SqlClient;

namespace DevRating.SqlClient
{
    internal sealed class FakeCommand : IDbCommand
    {
        private readonly FakeDataReader _reader;

        public FakeCommand(FakeDataReader reader)
        {
            _reader = reader;
        }
        
        public void Dispose()
        {
        }

        public void Cancel()
        {
        }

        public IDbDataParameter CreateParameter()
        {
            throw new System.NotImplementedException();
        }

        public int ExecuteNonQuery()
        {
            throw new System.NotImplementedException();
        }

        public IDataReader ExecuteReader()
        {
            return _reader;
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            throw new System.NotImplementedException();
        }

        public object ExecuteScalar()
        {
            throw new System.NotImplementedException();
        }

        public void Prepare()
        {
        }

        public string CommandText { get; set; } = string.Empty;
        public int CommandTimeout { get; set; }
        public CommandType CommandType { get; set; }
        public IDbConnection Connection { get; set; } = new SqlConnection();
        public IDataParameterCollection Parameters { get; } = new FakeDbParameterCollection();
        public IDbTransaction? Transaction { get; set; } = null;
        public UpdateRowSource UpdatedRowSource { get; set; }
    }
}