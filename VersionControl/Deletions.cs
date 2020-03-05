// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using DevRating.Domain;

namespace DevRating.VersionControl
{
    public interface Deletions
    {
        IEnumerable<Deletion> Items();
    }
}