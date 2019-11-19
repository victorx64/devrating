namespace DevRating.Database
{
    public interface Authors
    {
        IdentifiableAuthor Insert(string email);
        bool Exist(string email);
        IdentifiableAuthor Author(string email);
    }
}