using System;
using System.Collections.Generic;
using System.Linq;

namespace DevRating.Domain.Fake
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

        public Work Insert(string repository, string start, string end, Entity author, uint additions, Entity rating)
        {
            var work = new FakeWork(
                Guid.NewGuid(),
                additions,
                (Author) Entity(_authors, author.Id()),
                (Rating) Entity(_ratings, rating.Id()));

            _works.Add(work);

            return work;
        }

        public Work Insert(string repository, string start, string end, Entity author, uint additions)
        {
            var work = new FakeWork(
                Guid.NewGuid(),
                additions,
                (Author) Entity(_authors, author.Id()));

            _works.Add(work);

            return work;
        }

        public Work Insert(string repository, string start, string end, Entity author, uint additions, Entity rating,
            string link)
        {
            var work = new FakeWork(
                Guid.NewGuid(),
                additions,
                (Author) Entity(_authors, author.Id()),
                (Rating) Entity(_ratings, rating.Id()));

            _works.Add(work);

            return work;
        }

        public Work Insert(string repository, string start, string end, Entity author, uint additions, string link)
        {
            var work = new FakeWork(
                Guid.NewGuid(),
                additions,
                (Author) Entity(_authors, author.Id()));

            _works.Add(work);

            return work;
        }

        private Entity Entity(IEnumerable<Entity> entities, object id)
        {
            bool Predicate(Entity a)
            {
                return a.Id().Equals(id);
            }

            return entities.Single(Predicate);
        }
    }
}