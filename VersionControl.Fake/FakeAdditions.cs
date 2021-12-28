// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace DevRating.VersionControl.Fake
{
    public sealed class FakeAdditions : Additions
    {
        private readonly uint _count;

        public FakeAdditions(uint count)
        {
            _count = count;
        }

        public uint Count()
        {
            return _count;
        }
    }
}