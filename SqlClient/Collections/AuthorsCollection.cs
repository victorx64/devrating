using DevRating.SqlClient.Entities;

namespace DevRating.SqlClient.Collections
{
    internal interface AuthorsCollection
    {
        Author Insert(string email);
        bool Exist(string email);
        Author Author(string email);
    }
}