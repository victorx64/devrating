// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace DevRating.Domain
{
    public interface GetAuthorOperation
    {
        Author Author(string organization, string email);
        Author Author(Id id);
        IEnumerable<Author> TopOfOrganization(string organization, DateTimeOffset after);
        IEnumerable<Author> TopOfRepository(string repository, DateTimeOffset after);
    }
}