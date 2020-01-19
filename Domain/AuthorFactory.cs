namespace DevRating.Domain
{
    public interface AuthorFactory
    {
        Author Author(Entities entities);
        Author Author(Entities entities, string email);
    }
}