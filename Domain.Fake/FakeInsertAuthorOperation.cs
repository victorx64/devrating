using System;
using System.Collections.Generic;

namespace DevRating.Domain.Fake
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
            var author = new FakeAuthor(Guid.NewGuid(), email);

            _authors.Add(author);

            return author;
        }
    }
}