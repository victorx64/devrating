// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Data;

namespace DevRating.DefaultObject.Fake
{
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
            throw new System.NotImplementedException();
        }

        public int ExecuteNonQuery()
        {
            throw new System.NotImplementedException();
        }

        public IDataReader ExecuteReader()
        {
            throw new System.NotImplementedException();
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

        public string? CommandText { get; set; }
        public int CommandTimeout { get; set; }
        public CommandType CommandType { get; set; }
        public IDbConnection? Connection { get; set; }
        public IDataParameterCollection? Parameters { get; }
        public IDbTransaction? Transaction { get; set; }
        public UpdateRowSource UpdatedRowSource { get; set; }
    }
}