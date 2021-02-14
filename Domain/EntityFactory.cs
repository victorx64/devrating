// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface EntityFactory
    {
        Work InsertedWork(
            string organization,
            string repository,
            string start,
            string end,
            string? since,
            string email,
            uint additions,
            string? link, 
            DateTimeOffset createdAt
        );

        void InsertRatings(
            string organization,
            string repository,
            string email,
            IEnumerable<Deletion> deletions,
            Id work,
            DateTimeOffset createdAt
        );
    }
}