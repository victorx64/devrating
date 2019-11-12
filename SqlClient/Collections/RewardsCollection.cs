using System.Collections.Generic;
using DevRating.SqlClient.Entities;

namespace DevRating.SqlClient.Collections
{
    internal interface RewardsCollection
    {
        IdentifiableReward Insert(double value,
            string commit,
            string repository,
            uint count,
            IdentifiableRating rating,
            IdentifiableAuthor author);

        IdentifiableReward Insert(double value,
            string commit,
            string repository,
            uint count,
            IdentifiableAuthor author);

        IEnumerable<IdentifiableReward> RewardsOf(
            string commit,
            string repository);
    }
}