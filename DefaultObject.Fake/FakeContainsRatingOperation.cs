// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using DevRating.Domain;

namespace DevRating.DefaultObject.Fake
{
    public sealed class FakeContainsRatingOperation : ContainsRatingOperation
    {
        private readonly IList<Rating> _ratings;

        public FakeContainsRatingOperation(IList<Rating> ratings)
        {
            _ratings = ratings;
        }

        public bool ContainsRatingOf(Id author)
        {
            bool RatingOfAuthor(Rating r)
            {
                return r.Author().Id().Value().Equals(author.Value());
            }

            return _ratings.Any(RatingOfAuthor);
        }

        public bool Contains(Id id)
        {
            bool Rating(Rating r)
            {
                return r.Id().Value().Equals(id.Value());
            }

            return _ratings.Any(Rating);
        }
    }
}