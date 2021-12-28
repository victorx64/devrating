// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DevRating.Domain
{
    public interface Ratings
    {
        InsertRatingOperation InsertOperation();
        GetRatingOperation GetOperation();
        ContainsRatingOperation ContainsOperation();
    }
}