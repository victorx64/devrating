// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;

namespace DevRating.Domain
{
    public interface ContainsWorkOperation
    {
        bool Contains(string organization, string repository, string start, string end);
        bool Contains(string organization, string repository, DateTimeOffset after);
        bool Contains(Id id);
    }
}