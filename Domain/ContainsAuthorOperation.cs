namespace DevRating.Domain
{
    public interface ContainsAuthorOperation
    {
        bool Contains(string organization, string email);
        bool Contains(Id id);
    }
}