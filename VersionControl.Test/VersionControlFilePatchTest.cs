// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DevRating.VersionControl.Fake;
using Xunit;

namespace DevRating.VersionControl.Test
{
    public sealed class VersionControlFilePatchTest
    {
        [Fact]
        public void ReturnsDeletions()
        {
            Assert.NotNull(
                new VersionControlFilePatch(
                    "patch",
                    new FakeFileBlames()
                )
                .Deletions()
            );
        }

        [Fact]
        public void ReturnsAdditions()
        {
            Assert.NotNull(
                new VersionControlFilePatch(
                    "patch",
                    new FakeFileBlames()
                )
                .Additions()
            );
        }
    }
}