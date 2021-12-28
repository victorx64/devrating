// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using DevRating.Domain;

namespace DevRating.VersionControl
{
    public sealed class EmptyDeletions : Deletions
    {
        public IEnumerable<Deletion> Items()
        {
            return Array.Empty<Deletion>();
        }
    }
}