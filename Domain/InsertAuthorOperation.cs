namespace DevRating.Domain
{
    public interface InsertAuthorOperation
    {
        Author Insert(string email);
    }
}