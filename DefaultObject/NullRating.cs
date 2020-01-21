using System;
using DevRating.Domain;

namespace DevRating.DefaultObject
{
    public sealed class NullRating : Rating
    {
        private readonly double _value;

        public NullRating() : this(0d)
        {
        }

        public NullRating(double value)
        {
            _value = value;
        }

        public Id Id()
        {
            return new DefaultId();
        }

        public string ToJson()
        {
            throw new NotSupportedException();
        }

        public double Value()
        {
            return _value;
        }

        public Rating PreviousRating()
        {
            throw new NotSupportedException();
        }

        public Envelope<uint> Deletions()
        {
            throw new NotSupportedException();
        }

        public Work Work()
        {
            throw new NotSupportedException();
        }

        public Author Author()
        {
            throw new NotSupportedException();
        }
    }
}