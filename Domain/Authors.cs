using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface Authors
    {
        Author Insert(string email);
        bool Contains(string email);
        Author Author(string email);
        Author Author(object id);
        IEnumerable<Author> TopAuthors();
        IEnumerable<Author> TopAuthors(string repository);
        bool Contains(object id);
    }
}