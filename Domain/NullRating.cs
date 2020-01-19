using System;

namespace DevRating.Domain
{
    public sealed class NullRating : Rating    
    {
        public object Id()
        {
            return DBNull.Value;
        }

        public string ToJson()
        {
            throw new NotImplementedException();
        }

        public double Value()
        {
            throw new NotImplementedException();
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

        public uint Deletions()
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