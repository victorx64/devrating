using devrating.factory;
using devrating.git.fake;
using Xunit;

namespace devrating.git.test;

public sealed class TotalDeletionsTest
{
    [Fact]
    public void ReturnsTotalSumOfDeletions()
    {
        long Count(ContemporaryLines arg)
        {
            return arg.Size();
        }

        Assert.Equal(
            10u + 30u + 40u,
            new TotalDeletions(
                new FakePatches(
                    new[]
                    {
                        new FakeDeletions(
                            new[]
                            {
                                new GitContemporaryLines(100u, 10u, "first email"),
                            }),
                        new FakeDeletions(
                            new[]
                            {
                                new GitContemporaryLines(100u, 30u, "second email"),
                                new GitContemporaryLines(100u, 40u, "third email"),
                            }),
                    }
                )
            )
            .Items()
            .Sum(Count)
        );
    }

    [Fact]
    public void ReturnsThreeItemsOnThreeUniqueEmails()
    {
        Assert.Equal(
            3,
            new TotalDeletions(
                new FakePatches(
                    new[]
                    {
                        new FakeDeletions(
                            new[]
                            {
                                new GitContemporaryLines(100u, 10u, "email"),
                            }),
                        new FakeDeletions(
                            new[]
                            {
                                new GitContemporaryLines(100u, 30u, "some email"),
                                new GitContemporaryLines(100u, 40u, "some other email"),
                            }),
                    }
                )
            )
            .Items()
            .Count()
        );
    }

    [Fact]
    public void NotCombinesDeletionsWithTheSameEmail()
    {
        var email = "the email";

        Assert.Equal(
            3,
            new TotalDeletions(
                new FakePatches(
                    new[]
                    {
                        new FakeDeletions(
                            new[]
                            {
                                new GitContemporaryLines(100u, 10u, email),
                            }),
                        new FakeDeletions(
                            new[]
                            {
                                new GitContemporaryLines(100u, 30u, email),
                                new GitContemporaryLines(100u, 22u, "not the email"),
                            }),
                    }
                )
            )
            .Items()
            .Count()
        );
    }
}
