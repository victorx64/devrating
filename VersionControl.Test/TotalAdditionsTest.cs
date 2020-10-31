// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using DevRating.VersionControl.Fake;
using Xunit;

namespace DevRating.VersionControl.Test
{
    public sealed class TotalAdditionsTest
    {
        [Fact]
        public void ReturnsTotalSumOfAdditions()
        {
            Assert.Equal(10u + 11u + 12u,
                new TotalAdditions(
                    new FakePatches(
                        new[]
                        {
                            new FakeFilePatch(new EmptyDeletions(), new FakeAdditions(10u)),
                            new FakeFilePatch(new EmptyDeletions(), new FakeAdditions(11u)),
                            new FakeFilePatch(new EmptyDeletions(), new FakeAdditions(12u)),
                        }
                    )
                ).Count()
            );
        }
    }
}