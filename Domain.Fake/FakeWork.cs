using System.Collections.Generic;

namespace DevRating.Domain.Fake
{
    public sealed class FakeWork : Work
    {
        public object Id()
        {
            throw new System.NotImplementedException();
        }

        public string ToJson()
        {
            throw new System.NotImplementedException();
        }

        public uint Additions()
        {
            throw new System.NotImplementedException();
        }

        public Author Author()
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Rating> Ratings()
        {
            throw new System.NotImplementedException();
        }

        public Rating UsedRating()
        {
            throw new System.NotImplementedException();
        }

        public bool HasUsedRating()
        {
            throw new System.NotImplementedException();
        }
    }
}