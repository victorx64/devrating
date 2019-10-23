using DevRating.SqlClient.Entities;

namespace DevRating.SqlClient.Collections
{
    internal interface AuthorsCollection
    {
        Author NewAuthor(string email);
        bool Exist(string email);
        Author Author(string email);
    }
}