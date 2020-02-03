using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface EntityFactory
    {
        Work InsertedWork(string organization, string repository, string start, string end, string email,
            uint additions, Envelope link);
        void InsertRatings(string organization, string email, IEnumerable<Deletion> deletions, Id work);
    }
}