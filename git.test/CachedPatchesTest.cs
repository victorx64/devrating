using devrating.git.fake;
using Xunit;

namespace devrating.git.test;

public class CachedPatchesTest
{
    [Fact]
    public void ReturnsOriginItems()
    {
        var origin = new FakePatches(
            new[]
            {
                new FakeDeletions(
                    new[]
                    {
                        new GitContemporaryLines(2, 1, true, "some email"),
                    }
                ),
            }
        );

        Assert.Equal(new CachedPatches(origin).Items(), origin.Items());
    }

    [Fact]
    public void ReturnsItemsFasterOnSecondTime()
    {
        var cached = new CachedPatches(
            new DelayedPatches(
                new FakePatches(
                    new[]
                    {
                        new FakeDeletions(
                            new[]
                            {
                                new GitContemporaryLines(2, 2, true, "some other email"),
                            }
                        ),
                    }),
                TimeSpan.FromSeconds(0.1)
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
