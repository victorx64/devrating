using DevRating.SqlClient.Entities;

namespace DevRating.SqlClient.Collections
{
    internal interface AuthorsCollection
    {
        IdentifiableAuthor NewAuthor(string email);
        bool Exist(string email);
        IdentifiableAuthor Author(string email);
    }
}