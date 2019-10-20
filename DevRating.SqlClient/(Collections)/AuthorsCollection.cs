namespace DevRating.SqlClient
{
    internal interface AuthorsCollection
    {
        Author NewAuthor(string email);
        bool Exist(string email);
        Author Author(string email);
    }
}