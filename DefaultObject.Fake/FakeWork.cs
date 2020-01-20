using System;
using DevRating.Domain;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeWork : Work
    {
        private readonly object _id;
        private readonly uint _additions;
        private readonly Author _author;
        private readonly Rating _base;

        public FakeWork(uint additions, Author author)
            : this(additions, author, new NullRating())
        {
        }

        public FakeWork(uint additions, Author author, Rating @base)
            : this(Guid.NewGuid(), additions, author, @base)
        {
        }

        public FakeWork(object id, uint additions, Author author, Rating @base)
        {
            _id = id;
            _additions = additions;
            _author = author;
            _base = @base;
        }

        public object Id()
        {
            return _id;
        }

        public string ToJson()
        {
            throw new NotImplementedException();
        }

        public uint Additions()
        {
            return _additions;
        }

        public Author Author()
        {
            return _author;
        }

        public Rating UsedRating()
        {
            return _base;
        }

        public bool HasUsedRating()
        {
            return _base.Id() != DBNull.Value;
        }
    }
}