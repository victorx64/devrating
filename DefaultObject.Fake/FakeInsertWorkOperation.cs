using System;
using System.Collections.Generic;
using System.Linq;
using DevRating.Domain;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeInsertWorkOperation : InsertWorkOperation
    {
        private readonly IList<Work> _works;
        private readonly IList<Author> _authors;
        private readonly IList<Rating> _ratings;

        public FakeInsertWorkOperation(IList<Work> works, IList<Author> authors, IList<Rating> ratings)
        {
            _works = works;
            _authors = authors;
            _ratings = ratings;
        }

        public Work Insert(string repository, string start, string end, Entity author, uint additions, Entity rating,
            ObjectEnvelope link)
        {
            var work = new FakeWork(
                Guid.NewGuid(),
                additions,
                (Author) author,
                (Rating) rating
            );

            _works.Add(work);

            return work;
        }
    }
}