using System.Collections.Generic;

namespace DevRating.Domain.Fake
{
    public sealed class FakeWork : Work
    {
        private readonly object _id;
        private readonly uint _additions;
        private readonly Author _author;
        private readonly Rating? _rating;

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
            throw new System.NotImplementedException();
        }

        public uint Additions()
        {
            return _additions;
        }

        public Author Author()
        {
            return _author;
        }

        public IEnumerable<Rating> Ratings()
        {
            throw new System.NotImplementedException();
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