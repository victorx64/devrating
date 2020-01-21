namespace DevRating.Domain
{
    public interface ContainsAuthorOperation
    {
        bool Contains(string email);
        bool Contains(Id id);
    }
}