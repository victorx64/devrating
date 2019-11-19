using System;
using System.Collections.Generic;
using System.Data;

namespace DevRating.Database
{
    public sealed class FakeDataReader : IDataReader
    {
        private readonly IDictionary<string, object> _columns;
        private readonly string _command;

        public FakeDataReader(IDictionary<string, object> columns, string command)
        {
            _columns = columns;
            _command = command;
        }

        public object this[string name]
        {
            get { return _columns[name]; }
        }

        public bool Read()
        {
            var select = "SELECT";
            var from = "FROM";

            var start = _command.IndexOf(select, StringComparison.OrdinalIgnoreCase) + select.Length;
            var end = _command.IndexOf(from, StringComparison.OrdinalIgnoreCase);

            var columns = _command.Substring(start, end - start);

            foreach (var key in _columns.Keys)
            {
                if (columns.Contains(key, StringComparison.OrdinalIgnoreCase))
                {
                    return !(_columns[key] is DBNull);
                }
            }

            return false;
        }

        #region Not implemented members

        public int Depth { get; }
        public int FieldCount { get; }
        public bool IsClosed { get; }
        public int RecordsAffected { get; }

        public bool GetBoolean(int i)
        {
            throw new NotImplementedException();
        }

        public byte GetByte(int i)
        {
            throw new NotImplementedException();
        }

        public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public char GetChar(int i)
        {
            throw new NotImplementedException();
        }

        public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
        {
            throw new NotImplementedException();
        }

        public IDataReader GetData(int i)
        {
            throw new NotImplementedException();
        }

        public string GetDataTypeName(int i)
        {
            throw new NotImplementedException();
        }

        public DateTime GetDateTime(int i)
        {
            throw new NotImplementedException();
        }

        public decimal GetDecimal(int i)
        {
            throw new NotImplementedException();
        }

        public double GetDouble(int i)
        {
            throw new NotImplementedException();
        }

        public Type GetFieldType(int i)
        {
            throw new NotImplementedException();
        }

        public float GetFloat(int i)
        {
            throw new NotImplementedException();
        }

        public Guid GetGuid(int i)
        {
            throw new NotImplementedException();
        }

        public short GetInt16(int i)
        {
            throw new NotImplementedException();
        }

        public int GetInt32(int i)
        {
            throw new NotImplementedException();
        }

        public long GetInt64(int i)
        {
            throw new NotImplementedException();
        }

        public string GetName(int i)
        {
            throw new NotImplementedException();
        }

        public int GetOrdinal(string name)
        {
            throw new NotImplementedException();
        }

        public string GetString(int i)
        {
            throw new NotImplementedException();
        }

        public object GetValue(int i)
        {
            throw new NotImplementedException();
        }

        public int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public bool IsDBNull(int i)
        {
            throw new NotImplementedException();
        }

        public object this[int i] => throw new NotImplementedException();

        public void Dispose()
        {
        }

        public void Close()
        {
        }

        public DataTable GetSchemaTable()
        {
            throw new NotImplementedException();
        }

        public bool NextResult()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}