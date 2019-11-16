using DevRating.SqlClient.Entities;

namespace DevRating.SqlClient.Collections
{
    internal interface WorksCollection
    {
        IdentifiableWork Insert(
            string repository,
            string start,
            string end,
            IdentifiableObject author,
            double reward,
            IdentifiableObject rating);

        IdentifiableWork Insert(
            string repository,
            string start,
            string end,
            IdentifiableObject author,
            double reward);
    }
}