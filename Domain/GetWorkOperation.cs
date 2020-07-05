// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface GetWorkOperation
    {
        Work Work(string repository, string start, string end);
        Work Work(Id id);
        IEnumerable<Work> Last(string repository, DateTimeOffset after);
    }
}