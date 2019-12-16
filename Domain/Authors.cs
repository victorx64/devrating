using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface Authors
    {
        Author Insert(string email);
        bool Contains(string email);
        Author AuthorByEmail(string email);
        Author Author(string id);
        IEnumerable<Author> TopAuthors();
    }
}