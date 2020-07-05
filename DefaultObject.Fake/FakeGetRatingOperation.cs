// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using DevRating.Domain;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeGetRatingOperation : GetRatingOperation
    {
        private readonly IList<Rating> _ratings;

        public FakeGetRatingOperation(IList<Rating> ratings)
        {
            _ratings = ratings;
        }

        public Rating RatingOf(Id author)
        {
            bool Predicate(Rating a)
            {
                return a.Author().Id().Value().Equals(author.Value());
            }

            if (!_ratings.Any(Predicate))
            {
                return new NullRating();
            }

            return _ratings.Last(Predicate);
        }

        public Rating Rating(Id id)
        {
            bool Predicate(Entity a)
            {
                return a.Id().Value().Equals(id.Value());
            }

            if (!_ratings.Any(Predicate))
            {
                return new NullRating();
            }

            return _ratings.Single(Predicate);
        }

        public IEnumerable<Rating> RatingsOf(Id work)
        {
            bool Predicate(Rating rating)
            {
                return rating.Work().Id().Equals(work);
            }

            return _ratings.Where(Predicate);
        }

        public IEnumerable<Rating> Last(Id author, DateTimeOffset after)
        {
            bool Predicate(Rating rating)
            {
                return rating.Author().Id().Equals(author) &&
                    rating.CreatedAt() >= after;
            }

            return _ratings.Where(Predicate);
        }
    }
}