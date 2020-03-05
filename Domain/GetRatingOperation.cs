// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface GetRatingOperation
    {
        Rating RatingOf(Id author);
        Rating Rating(Id id);
        IEnumerable<Rating> RatingsOf(Id work);
    }
}