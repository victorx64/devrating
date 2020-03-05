// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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