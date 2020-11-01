// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevRating.VersionControl.Fake
{
    public sealed class DelayedPatches : Patches
    {
        private readonly Patches _origin;
        private readonly TimeSpan _delay;

        public DelayedPatches(Patches origin, TimeSpan delay)
        {
            _origin = origin;
            _delay = delay;
        }

        public IEnumerable<FilePatch> Items()
        {
            Task.Delay(_delay).ConfigureAwait(false).GetAwaiter().GetResult();

            return _origin.Items();
        }
    }
}