using System.Collections.Generic;
using DevRating.Domain;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeInsertAuthorOperation : InsertAuthorOperation
    {
        private readonly IList<Author> _authors;

        public FakeInsertAuthorOperation(IList<Author> authors)
        {
            _authors = authors;
        }

        public Author Insert(string email)
        {
            var author = new FakeAuthor(email);

            _authors.Add(author);

            return author;
        }
    }
}