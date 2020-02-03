namespace DevRating.Domain
{
    public interface InsertAuthorOperation
    {
        Author Insert(string organization, string email);
    }
}