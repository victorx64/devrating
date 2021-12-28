// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface GetAuthorOperation
    {
        Author Author(string organization, string repository, string email);
        Author Author(Id id);
        IEnumerable<Author> Top(string organization, string repository, DateTimeOffset after);
    }
}