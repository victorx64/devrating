using System;
using DevRating.Domain;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeWork : Work
    {
        private readonly Id _id;
        private readonly uint _additions;
        private readonly Author _author;
        private readonly Rating _rating;

        public FakeWork(uint additions, Author author)
            : this(additions, author, new NullRating())
        {
        }

        public FakeWork(uint additions, Author author, Rating rating)
            : this(new DefaultId(Guid.NewGuid()), additions, author, rating)
        {
        }

        public FakeWork(Id id, uint additions, Author author, Rating rating)
        {
            _id = id;
            _additions = additions;
            _author = author;
            _rating = rating;
        }

        public Id Id()
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
            return _rating;
        }
    }
}