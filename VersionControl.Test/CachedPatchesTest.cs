// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using DevRating.DefaultObject;
using DevRating.VersionControl.Fake;
using Xunit;

namespace DevRating.VersionControl.Test
{
    public class CachedPatchesTest
    {
        [Fact]
        public void ReturnsOriginItems()
        {
            var origin = new FakePatches(
                new[]
                {
                    new FakeFilePatch(
                        new FakeDeletions(
                            new[]
                            {
                                new DefaultDeletion("some email", 1),
                            }),
                        new FakeAdditions(2)
                    ),
                }
            );

            Assert.Equal(new CachedPatches(origin).Items(), origin.Items());
        }

        [Fact]
        public void ReturnsItemsFasterASecondTime()
        {
            var cached = new CachedPatches(
                new DelayedPatches(
                    new FakePatches(
                        new[]
                        {
                            new FakeFilePatch(
                                new FakeDeletions(
                                    new[]
                                    {
                                        new DefaultDeletion("some other email", 2),
                                    }
                                ),
                                new FakeAdditions(3)
                            ),
                        }),
                    TimeSpan.FromSeconds(1)
                )
            );

            var first = DateTime.Now;

            cached.Items();

            var second = DateTime.Now;

            cached.Items();

            var third = DateTime.Now;

            Assert.True(second - first > third - second);
        }
    }
}