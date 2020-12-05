// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace DevRating.VersionControl
{
    public sealed class CachedPatches : Patches
    {
        private readonly Patches _origin;
        private readonly object _lock = new object();
        private IEnumerable<FilePatch>? _items;

        public CachedPatches(Patches origin)
        {
            _origin = origin;
        }

        public IEnumerable<FilePatch> Items()
        {
            if (_items is object)
            {
                return _items;
            }

            lock (_lock)
            {
                return _items ??= _origin.Items();
            }
        }
    }
}