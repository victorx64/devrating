// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Xunit;

namespace DevRating.VersionControl.Test
{
    public sealed class VersionControlProcessTest
    {
        [Fact]
        public void ReturnsOutputLines()
        {
            Assert.Equal(2, new VersionControlProcess("dotnet", "--version", ".").Output().Count);
        }
    }
}