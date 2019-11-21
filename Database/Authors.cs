using System.Collections.Generic;

namespace DevRating.Database
{
    public interface Authors
    {
        DbAuthor Insert(string email);
        bool Exist(string email);
        DbAuthor Author(string email);
        IEnumerable<DbAuthor> TopAuthors();
    }
}