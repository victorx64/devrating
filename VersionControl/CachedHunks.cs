// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace DevRating.VersionControl
{
    public sealed class CachedHunks : Hunks
    {
        private readonly Hunks _origin;
        private IEnumerable<Hunk>? _items;

        public CachedHunks(Hunks origin)
        {
            _origin = origin;
        }

        public IEnumerable<Hunk> Items()
        {
            lock (_origin)
            {
                return _items ??= _origin.Items();
            }
        }
    }
}