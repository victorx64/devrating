// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using DevRating.Domain;

namespace DevRating.VersionControl.Fake
{
    public sealed class FakeDeletions : Deletions
    {
        private readonly IEnumerable<Deletion> _items;

        public FakeDeletions(IEnumerable<Deletion> items)
        {
            _items = items;
        }

        public IEnumerable<Deletion> Items()
        {
            return _items;
        }
    }
}