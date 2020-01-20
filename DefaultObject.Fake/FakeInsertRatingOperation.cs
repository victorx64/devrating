using System;
using System.Collections.Generic;
using DevRating.Domain;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeInsertRatingOperation : InsertRatingOperation
    {
        private readonly IList<Rating> _ratings;

        public FakeInsertRatingOperation(IList<Rating> ratings)
        {
            _ratings = ratings;
        }

        public Rating Insert(double value, ObjectEnvelope deletions, Entity previous, Entity work, Entity author)
        {
            var rating = new FakeRating(
                Guid.NewGuid(),
                value,
                (Work) work,
                (Author) author,
                (Rating) previous,
                deletions
            );

            _ratings.Add(rating);

            return rating;
        }
    }
}