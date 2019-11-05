using System.Collections.Generic;
using DevRating.SqlClient.Entities;

namespace DevRating.SqlClient.Collections
{
    internal interface AuthorsCollection
    {
        SqlAuthor NewAuthor(string email);
        bool Exist(string email);
        SqlAuthor Author(string email);
        IEnumerable<SqlAuthor> TopAuthors();
    }
}