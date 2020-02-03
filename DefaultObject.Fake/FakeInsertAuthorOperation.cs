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

        public Author Insert(string organization, string email)
        {
            var author = new FakeAuthor(organization, email);

            _authors.Add(author);

            return author;
        }
    }
}