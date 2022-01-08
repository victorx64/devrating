using System.Data;

namespace devrating.consoleapp.fake;

public sealed class FakeDbCommand : IDbCommand
{
    public void Dispose()
    {
    }

    public void Cancel()
    {
    }

    public IDbDataParameter CreateParameter()
    {
        throw new NotImplementedException();
    }

    public int ExecuteNonQuery()
    {
        throw new NotImplementedException();
    }

    public IDataReader ExecuteReader()
    {
        throw new NotImplementedException();
    }

    public IDataReader ExecuteReader(CommandBehavior behavior)
    {
        throw new NotImplementedException();
    }

    public object ExecuteScalar()
    {
        throw new NotImplementedException();
    }

    public void Prepare()
    {
    }

#nullable disable
    public string CommandText { get; set; }
    public IDataParameterCollection Parameters { get; }
#nullable enable
    public int CommandTimeout { get; set; }
    public CommandType CommandType { get; set; }
    public IDbConnection? Connection { get; set; }
    public IDbTransaction? Transaction { get; set; }
    public UpdateRowSource UpdatedRowSource { get; set; }
}
