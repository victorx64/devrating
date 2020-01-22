using System;
using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface EntityFactory
    {
        Work InsertedWork(string repository, string start, string end, string email, uint additions, Envelope<IConvertible> link);
        void InsertRatings(string email, IEnumerable<Deletion> deletions, Id work);
    }
}