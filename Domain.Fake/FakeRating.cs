namespace DevRating.Domain.Fake
{
    public sealed class FakeRating : Rating
    {
        private readonly object _id;
        private readonly double _value;
        private readonly Work _work;
        private readonly Author _author;
        private readonly Rating? _previous;

        public FakeRating(object id, double value, Work work, Author author)
            : this(id, value, work, author, null)
        {
        }

        public FakeRating(object id, double value, Work work, Author author, Rating previous)
        {
            _id = id;
            _value = value;
            _work = work;
            _author = author;
            _previous = previous;
        }

        public object Id()
        {
            return _id;
        }

        public string ToJson()
        {
            throw new System.NotImplementedException();
        }

        public double Value()
        {
            return _value;
        }

        public bool HasPreviousRating()
        {
            return _previous != null;
        }

        public Rating PreviousRating()
        {
            return _previous;
        }

        public Work Work()
        {
            return _work;
        }

        public Author Author()
        {
            return _author;
        }
    }
}