// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace DevRating.Domain
{
    public interface InsertWorkOperation
    {
        Work Insert(
            string repository,
            string start,
            string end,
            Envelope since,
            Id author,
            uint additions,
            Id rating,
            Envelope link,
            DateTimeOffset createdAt
        );
    }
}