using System;
using System.Collections.Generic;
using System.Linq;

namespace DevRating.Domain.Fake
{
    public sealed class FakeInsertRatingOperation : InsertRatingOperation
    {
        private readonly IList<Work> _works;
        private readonly IList<Author> _authors;
        private readonly IList<Rating> _ratings;

        public FakeInsertRatingOperation(IList<Work> works, IList<Author> authors, IList<Rating> ratings)
        {
            _works = works;
            _authors = authors;
            _ratings = ratings;
        }

        public Rating Insert(Entity author, double value, Entity work)
        {
            var rating = new FakeRating(
                Guid.NewGuid(),
                value,
                (Work) Entity(_works, work.Id()),
                (Author) Entity(_authors, author.Id()));

            _ratings.Add(rating);

            return rating;
        }

        public Rating Insert(Entity author, double value, Entity previous, Entity work)
        {
            var rating = new FakeRating(
                Guid.NewGuid(),
                value,
                (Work) Entity(_works, work.Id()),
                (Author) Entity(_authors, author.Id()),
                (Rating) Entity(_ratings, previous.Id()));

            _ratings.Add(rating);

            return rating;
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