// Copyright (c) 2019-present Victor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Semver;
using Xunit;

namespace DevRating.VersionControl.Test
{
    public sealed class VersionControlTagTest
    {
        [Fact]
        public void ReturnsSha()
        {
            var expected = "sha";

            Assert.Equal(expected, new VersionControlTag(expected, string.Empty).Sha());
        }

        [Fact]
        public void ReturnsEnvelopedSha()
        {
            var expected = "sha";

            Assert.Equal(expected, new VersionControlTag(expected, null as SemVersion).Sha());
        }

        [Fact]
        public void ReturnsInjectedVersion()
        {
            var expected = new SemVersion(3, 4, 5);

            Assert.Equal(expected, new VersionControlTag("sha", expected).Version());
        }

        [Fact]
        public void HasVersionWhenInjected()
        {
            Assert.True(new VersionControlTag("sha", new SemVersion(3, 4, 5)).Version() is object);
        }

        [Fact]
        public void ParsesVersionWithV()
        {
            Assert.True(new VersionControlTag("sha", "v1.0.0").Version() is object);
        }

        [Fact]
        public void ParsesVersionWithCapitalV()
        {
            Assert.True(new VersionControlTag("sha", "V1.0.0").Version() is object);
        }

        [Fact]
        public void ParsesVersionWithoutV()
        {
            Assert.True(new VersionControlTag("sha", "1.0.0").Version() is object);
        }

        [Fact]
        public void DoesNotParseNameWithOtherPrefix()
        {
            Assert.False(new VersionControlTag("sha", "_1.0.0").Version() is object);
        }

        [Fact]
        public void DoesNotParseNameWithoutVersion()
        {
            Assert.False(new VersionControlTag("sha", "random-text").Version() is object);
        }

        [Fact]
        public void ReturnsParsedVersion()
        {
            Assert.Equal(new SemVersion(2, 3, 5), new VersionControlTag("sha", "2.3.5").Version());
        }
    }
}