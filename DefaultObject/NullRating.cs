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
            throw new NotImplementedException();
        }

        public double Value()
        {
            return _value;
        }

        public bool HasPreviousRating()
        {
            throw new NotImplementedException();
        }

        public Rating PreviousRating()
        {
            throw new NotImplementedException();
        }

        public bool HasDeletions()
        {
            throw new NotImplementedException();
        }

        public Envelope<uint> Deletions()
        {
            throw new NotImplementedException();
        }

        public Work Work()
        {
            throw new NotImplementedException();
        }

        public Author Author()
        {
            throw new NotImplementedException();
        }
    }
}