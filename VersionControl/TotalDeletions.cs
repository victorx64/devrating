// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Linq;
using DevRating.DefaultObject;
using DevRating.Domain;

namespace DevRating.VersionControl
{
    public sealed class TotalDeletions : Deletions
    {
        private readonly Patches _patches;

        public TotalDeletions(Patches patches)
        {
            _patches = patches;
        }

        public IEnumerable<Deletion> Items()
        {
            return _patches.Items()
                .SelectMany(p => p.Deletions().Items());
        }
    }
}