using System.Collections.Generic;
using DevRating.Domain;

namespace DevRating.Database
{
    public sealed class DbAuthorsRepository : AuthorsRepository
    {
        private readonly Authors _authors;

        public DbAuthorsRepository(Authors authors)
        {
            _authors = authors;
        }

        public IEnumerable<Author> TopAuthors()
        {
            return _authors.TopAuthors();
        }
    }
}