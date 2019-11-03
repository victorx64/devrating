using System.Linq;
using DevRating.SqlClient.Entities;

namespace DevRating.SqlClient.Collections
{
    internal interface AuthorsCollection
    {
        SqlAuthor NewAuthor(string email);
        bool Exist(string email);
        SqlAuthor Author(string email);
        IOrderedEnumerable<SqlAuthor> TopAuthors();
    }
}