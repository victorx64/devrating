using System.Collections.Generic;
using DevRating.Domain;

namespace DevRating.WebApp
{
    public sealed class Application
    {
        private readonly AuthorsRepository _authors;

        public Application(AuthorsRepository authors)
        {
            _authors = authors;
        }
        
        public IEnumerable<Author> TopAuthors()
        {
            return _authors.TopAuthors();
        }
    }
}