using System;
using System.Collections.Generic;

namespace DevRating.Domain.Fake
{
    public sealed class FakeWork : Work
    {
        private readonly object _id;
        private readonly uint _additions;
        private readonly Author _author;
        private readonly Rating? _rating;

        public FakeWork(uint additions, Author author)
            : this(Guid.NewGuid(), additions, author, null)
        {
        }

        public FakeWork(uint additions, Author author, Rating? rating)
            : this(Guid.NewGuid(), additions, author, rating)
        {
        }

        public FakeWork(object id, uint additions, Author author)
            : this(id, additions, author, null)
        {
        }

        public FakeWork(object id, uint additions, Author author, Rating? rating)
        {
            _id = id;
            _additions = additions;
            _author = author;
            _rating = rating;
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
            return _rating!;
        }

        public bool HasUsedRating()
        {
            return _rating != null;
        }
    }
}