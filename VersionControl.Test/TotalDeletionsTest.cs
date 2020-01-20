using System.Linq;
using DevRating.Domain;
using DevRating.VersionControl.Fake;
using Xunit;

namespace DevRating.VersionControl.Test
{
    public sealed class TotalDeletionsTest
    {
        [Fact]
        public void ReturnsTotalSumOfDeletions()
        {
            long Count(Deletion arg)
            {
                return arg.Count();
            }

            Assert.Equal(
                10u + 30u + 40u,
                new TotalDeletions(
                        new FakeHunks(
                            new[]
                            {
                                new FakeHunk(
                                    new FakeDeletions(
                                        new[]
                                        {
                                            new VersionControlDeletion("first email", 10u),
                                        }),
                                    new FakeAdditions(2u)),
                                new FakeHunk(
                                    new FakeDeletions(
                                        new[]
                                        {
                                            new VersionControlDeletion("second email", 30u),
                                            new VersionControlDeletion("third email", 40u),
                                        }),
                                    new FakeAdditions(5u)),
                            }
                        )
                    )
                    .Items()
                    .Sum(Count));
        }

        [Fact]
        public void ReturnsThreeItemsOnThreeUniqueEmails()
        {
            Assert.Equal(
                3,
                new TotalDeletions(
                        new FakeHunks(
                            new[]
                            {
                                new FakeHunk(
                                    new FakeDeletions(
                                        new[]
                                        {
                                            new VersionControlDeletion("email", 10u),
                                        }),
                                    new FakeAdditions(2u)),
                                new FakeHunk(
                                    new FakeDeletions(
                                        new[]
                                        {
                                            new VersionControlDeletion("some email", 30u),
                                            new VersionControlDeletion("some other email", 40u),
                                        }),
                                    new FakeAdditions(5u)),
                            }
                        )
                    )
                    .Items()
                    .Count());
        }

        [Fact]
        public void CombinesDeletionsWithTheSameEmail()
        {
            var email = "the email";

            bool DeletionWithTheEmail(Deletion deletion)
            {
                return deletion.Email().Equals(email);
            }

            Assert.Equal(
                10u + 30u,
                new TotalDeletions(
                        new FakeHunks(
                            new[]
                            {
                                new FakeHunk(
                                    new FakeDeletions(
                                        new[]
                                        {
                                            new VersionControlDeletion(email, 10u),
                                        }),
                                    new FakeAdditions(2u)),
                                new FakeHunk(
                                    new FakeDeletions(
                                        new[]
                                        {
                                            new VersionControlDeletion(email, 30u),
                                            new VersionControlDeletion("not the email", 40u),
                                        }),
                                    new FakeAdditions(5u)),
                            }
                        )
                    )
                    .Items()
                    .Single(DeletionWithTheEmail)
                    .Count());
        }

        [Fact]
        public void IgnoresCaseOfEmailWhenCombining()
        {
            Assert.Single(
                new TotalDeletions(
                        new FakeHunks(
                            new[]
                            {
                                new FakeHunk(
                                    new FakeDeletions(
                                        new[]
                                        {
                                            new VersionControlDeletion("email@domain", 10u), 
                                        }),
                                    new FakeAdditions(2u)),
                                new FakeHunk(
                                    new FakeDeletions(
                                        new[]
                                        {
                                            new VersionControlDeletion("EMAIL@domain", 30u),
                                        }),
                                    new FakeAdditions(5u)),
                            }
                        )
                    )
                    .Items()
            );
        }
    }
}