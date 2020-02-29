using System;
using DevRating.Domain;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeRating : Rating
    {
        private readonly Id _id;
        private readonly double _value;
        private readonly Work _work;
        private readonly Author _author;
        private readonly Rating _previous;
        private readonly Envelope _deletions;
        private readonly Envelope _ignored;

        public FakeRating(double value, Work work, Author author)
            : this(value, work, author, new NullRating(), new DefaultEnvelope(), new DefaultEnvelope())
        {
        }

        public FakeRating(double value, Work work, Author author, Rating previous, Envelope deletions, Envelope ignored)
            : this(new DefaultId(Guid.NewGuid()), value, work, author, previous, deletions, ignored)
        {
        }

        public FakeRating(Id id, double value, Work work, Author author, Rating previous, Envelope deletions, Envelope ignored)
        {
            _id = id;
            _value = value;
            _work = work;
            _author = author;
            _previous = previous;
            _deletions = deletions;
            _ignored = ignored;
        }

        public Id Id()
        {
            return _id;
        }

        public string ToJson()
        {
            throw new NotImplementedException();
        }

        public double Value()
        {
            return _value;
        }

        public Rating PreviousRating()
        {
            return _previous;
        }

        public Envelope CountedDeletions()
        {
            return _deletions;
        }

        public Envelope IgnoredDeletions()
        {
            return _ignored;
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