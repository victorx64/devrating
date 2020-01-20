
namespace DevRating.Domain
{
    public interface EntitiesFactory
    {
        Work InsertedWork(string repository, string start, string end, string email, uint additions);
        void InsertRatings(string email, Deletions deletions, Entity work);
    }
}