namespace DevRating.Domain
{
    public interface ContainsWorkOperation
    {
        bool Contains(string repository, string start, string end);
        bool Contains(Id id);
    }
}