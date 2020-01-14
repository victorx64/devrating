namespace DevRating.Domain.Fake
{
    public sealed class FakeAuthor : Author
    {
        private readonly object _id;
        private readonly string _email;
        private readonly Rating? _rating;

        public FakeAuthor(object id, string email)
            : this(id, email, null)
        {
        }

        public FakeAuthor(object id, string email, Rating? rating)
        {
            _id = id;
            _email = email;
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

        public string Email()
        {
            return _email;
        }

        public Rating Rating()
        {
            return _rating!;
        }

        public bool HasRating()
        {
            return _rating != null;
        }
    }
}