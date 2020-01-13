using System;
using DevRating.Domain.Fake;
using Xunit;

namespace DevRating.Domain.Test
{
    public class CachedHunksTest
    {
        [Fact]
        public void ReturnsOriginItems()
        {
            var origin = new FakeHunks(
                new[]
                {
                    new FakeHunk(
                        new FakeDeletions(
                            new[]
                            {
                                new FakeDeletion("some email", 1),
                            }),
                        new FakeAdditions(2)),
                });

            Assert.Equal(new CachedHunks(origin).Items(), origin.Items());
        }

        [Fact]
        public void ReturnsItemsFasterASecondTime()
        {
            var cached = new CachedHunks(
                new DelayedHunks(
                    new FakeHunks(
                        new[]
                        {
                            new FakeHunk(
                                new FakeDeletions(
                                    new[]
                                    {
                                        new FakeDeletion("some other email", 2),
                                    }),
                                new FakeAdditions(3)),
                        }),
                    TimeSpan.FromSeconds(2)));

            var first = DateTime.Now;

            cached.Items();

            var second = DateTime.Now;

            cached.Items();

            var third = DateTime.Now;

            Assert.True(second - first > third - second);
        }
    }
}