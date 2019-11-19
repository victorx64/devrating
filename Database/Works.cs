using DevRating.Domain;

namespace DevRating.Database
{
    public interface Works
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

        IdentifiableWork Work(WorkKey key);
        bool Exist(WorkKey key);
    }
}