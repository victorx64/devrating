namespace DevRating.SqlClient
{
    internal interface Authors
    {
        IdentifiableAuthor Insert(string email);
        bool Exist(string email);
        IdentifiableAuthor Author(string email);
    }
}