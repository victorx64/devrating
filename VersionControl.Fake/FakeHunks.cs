// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace DevRating.VersionControl.Fake
{
    public sealed class FakeHunks : Hunks
    {
        private readonly IEnumerable<Hunk> _items;

        public FakeHunks(IEnumerable<Hunk> items)
        {
            _items = items;
        }

        public IEnumerable<Hunk> Items()
        {
            return _items;
        }
    }
}