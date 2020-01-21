using System;
using DevRating.Domain;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeAuthor : Author
    {
        private readonly Id _id;
        private readonly string _email;

        public FakeAuthor(string email) : this(new DefaultId(Guid.NewGuid()), email)
        {
        }

        public FakeAuthor(Id id, string email)
        {
            _id = id;
            _email = email;
        }

        public Id Id()
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