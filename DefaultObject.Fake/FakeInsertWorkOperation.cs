using System;
using System.Collections.Generic;
using System.Linq;
using DevRating.Domain;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeInsertWorkOperation : InsertWorkOperation
    {
        private readonly IList<Work> _works;
        private readonly IList<Rating> _ratings;
        private readonly IList<Author> _authors;

        public FakeInsertWorkOperation(IList<Work> works, IList<Rating> ratings, IList<Author> authors)
        {
            _works = works;
            _ratings = ratings;
            _authors = authors;
        }

        public Work Insert(string repository, string start, string end, Id author, uint additions, Id rating,
            Envelope<string> link)
        {
            var work = new FakeWork(
                new DefaultId(Guid.NewGuid()),
                additions,
                Author(author),
                Rating(rating)
            );

            _works.Add(work);

            return work;
        }

        private Rating Rating(Id id)
        {
            if (!id.Filled())
            {
                return new NullRating();
            }

            bool Predicate(Entity e)
            {
                return e.Id().Value().Equals(id.Value());
            }

            return _ratings.Single(Predicate);
        }

        private Author Author(Id id)
        {
            bool Predicate(Entity e)
            {
                return e.Id().Value().Equals(id.Value());
            }

            return _authors.Single(Predicate);
        }
    }
}