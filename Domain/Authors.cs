namespace DevRating.Domain
{
    public interface Authors
    {
        GetAuthorOperation GetOperation();
        InsertAuthorOperation InsertOperation();
        ContainsAuthorOperation ContainsOperation();
    }
}