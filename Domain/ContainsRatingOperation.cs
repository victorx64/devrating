// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DevRating.Domain
{
    public interface ContainsRatingOperation
    {
        bool ContainsRatingOf(Id author);
        bool Contains(Id id);
    }
}