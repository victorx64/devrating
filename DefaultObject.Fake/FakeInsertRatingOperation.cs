using System;
using System.Collections.Generic;
using System.Linq;
using DevRating.Domain;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeInsertRatingOperation : InsertRatingOperation
    {
        private readonly IList<Work> _works;
        private readonly IList<Rating> _ratings;
        private readonly IList<Author> _authors;

        public FakeInsertRatingOperation(IList<Rating> ratings, IList<Work> works, IList<Author> authors)
        {
            _ratings = ratings;
            _works = works;
            _authors = authors;
        }

        public Rating Insert(double value, Envelope deletions, Id previous, Id work, Id author)
        {
            var rating = new FakeRating(
                new DefaultId(Guid.NewGuid()),
                value,
                Work(work),
                Author(author),
                Rating(previous),
                deletions
            );

            _ratings.Add(rating);

            return rating;
        }

        private Rating Rating(Id id)
        {
            if (id.Value().Equals(DBNull.Value))
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

        private Work Work(Id id)
        {
            bool Predicate(Entity e)
            {
                return e.Id().Value().Equals(id.Value());
            }

            return _works.Single(Predicate);
        }
    }
}