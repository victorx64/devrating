// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface GetRatingOperation
    {
        Rating RatingOf(Id author);
        Rating Rating(Id id);
        IEnumerable<Rating> RatingsOf(Id work);
        IEnumerable<Rating> Last(Id author, DateTimeOffset after);
    }
}