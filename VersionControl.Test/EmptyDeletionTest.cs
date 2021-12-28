// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace DevRating.VersionControl.Test
{
    public sealed class EmptyDeletionTest
    {
        [Fact]
        public void ReturnsEmptyItems()
        {
            Assert.Empty(new EmptyDeletions().Items());
        }
    }
}