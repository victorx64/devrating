// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace DevRating.VersionControl.Test
{
    public sealed class CountedBlameTest
    {
        [Fact]
        public void ContainsLine()
        {
            Assert.True(new AccountableBlame("email", 1, 1).ContainsLine(1));
        }

        [Fact]
        public void DoesNotContainLineBefore()
        {
            Assert.False(new AccountableBlame("email", 1, 1).ContainsLine(0));
        }

        [Fact]
        public void DoesNotContainLineAfter()
        {
            Assert.False(new AccountableBlame("email", 1, 1).ContainsLine(2));
        }

        [Fact]
        public void ReturnsZeroIgnoredLines()
        {
            Assert.Equal(0u, new AccountableBlame("email", 1, 1).SubDeletion(0, 100).Ignored());
        }

        [Fact]
        public void ReturnsAllCountedLinesOnBigDeletionRequest()
        {
            Assert.Equal(10u, new AccountableBlame("email", 1, 10).SubDeletion(0, 100).Lines());
        }
    }
}