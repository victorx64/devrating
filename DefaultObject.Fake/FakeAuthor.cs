using System;
using DevRating.Domain;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeAuthor : Author
    {
        private readonly object _id;
        private readonly string _email;

        public FakeAuthor(string email) : this(Guid.NewGuid(), email)
        {
        }

        public FakeAuthor(object id, string email)
        {
            _id = id;
            _email = email;
        }

        public object Id()
        {
            return _id;
        }

        public string ToJson()
        {
            throw new NotImplementedException();
        }

        public string Email()
        {
            return _email;
        }
    }
}