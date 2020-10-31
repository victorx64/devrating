// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace DevRating.VersionControl.Fake
{
    public sealed class FakePatches : Patches
    {
        private readonly IEnumerable<FilePatch> _items;

        public FakePatches(IEnumerable<FilePatch> items)
        {
            _items = items;
        }

        public IEnumerable<FilePatch> Items()
        {
            return _items;
        }
    }
}