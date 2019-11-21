using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface AuthorsRepository
    {
        IEnumerable<Author> TopAuthors();
    }
}