using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface Authors
    {
        Author Insert(string email);
        bool Contains(string email);
        Author Author(string email);
        IEnumerable<Author> TopAuthors();
    }
}