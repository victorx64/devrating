using System;
using System.Collections;
using System.Data.Common;

namespace DevRating.SqlClient
{
    internal sealed class FakeDbParameterCollection : DbParameterCollection
    {
        public override int Add(object value)
        {
            return 1;
        }

        public override void Clear()
        {
        }

        public override bool Contains(object value)
        {
            throw new NotImplementedException();
        }

        public override int IndexOf(object value)
        {
            throw new NotImplementedException();
        }

        public override void Insert(int index, object value)
        {
        }

        public override void Remove(object value)
        {
        }

        public override void RemoveAt(int index)
        {
        }

        public override void RemoveAt(string parameterName)
        {
        }

        protected override void SetParameter(int index, DbParameter value)
        {
        }

        protected override void SetParameter(string parameterName, DbParameter value)
        {
        }

        public override int Count { get; }
        public override object SyncRoot { get; } = new object();

        public override int IndexOf(string parameterName)
        {
            throw new NotImplementedException();
        }

        public override bool Contains(string value)
        {
            throw new NotImplementedException();
        }

        public override void CopyTo(Array array, int index)
        {
        }

        public override IEnumerator GetEnumerator()
        {
            throw new NotImplementedException();
        }

        protected override DbParameter GetParameter(int index)
        {
            throw new NotImplementedException();
        }

        protected override DbParameter GetParameter(string parameterName)
        {
            throw new NotImplementedException();
        }

        public override void AddRange(Array values)
        {
        }
    }
}