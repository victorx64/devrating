using DevRating.VersionControl.Fake;
using Xunit;

namespace DevRating.VersionControl.Test
{
    public sealed class GitProcessBlamesTest
    {
        [Fact]
        public void CombinesLinesWithSameRevision()
        {
            var collection = new GitProcessBlames(
                new FakeProcess(
@"^7f30cd2 (<svikk@live.ru>              1595608723 +0900  1) sv
^7f30cd2 (<svikk@live.ru>              1595608723 +0900  2) sv
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  3) sv1
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  4) sv1
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  5) sv11
"
)
            );

            Assert.Equal(
                3u,
                collection.AtLine(2).SubDeletion(0, 1000).Ignored() +
                collection.AtLine(2).SubDeletion(0, 1000).Counted()
            );
        }

        [Fact]
        public void IgnoresLinesThatStartWithSpecialChar()
        {
            Assert.Equal(
                2u,
                new GitProcessBlames(
                    new FakeProcess(
@"^7f30cd2 (<svikk@live.ru>              1595608723 +0900  1) sv
^7f30cd2 (<svikk@live.ru>              1595608723 +0900  2) sv
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  3) sv1
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  4) sv1
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  5) sv11
"
)
                )
                .AtLine(0)
                .SubDeletion(0, 1000)
                .Ignored()
            );
        }

        [Fact]
        public void DoesNotCountLinesThatStartWithSpecialChar()
        {
            Assert.Equal(
                0u,
                new GitProcessBlames(
                    new FakeProcess(
@"^7f30cd2 (<svikk@live.ru>              1595608723 +0900  1) sv
^7f30cd2 (<svikk@live.ru>              1595608723 +0900  2) sv
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  3) sv1
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  4) sv1
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  5) sv11
"
)
                )
                .AtLine(0)
                .SubDeletion(0, 1000)
                .Counted()
            );
        }

        [Fact]
        public void CountsLinesThatStartWithAlphanumericalChar()
        {
            Assert.Equal(
                3u,
                new GitProcessBlames(
                    new FakeProcess(
@"^7f30cd2 (<svikk@live.ru>              1595608723 +0900  1) sv
^7f30cd2 (<svikk@live.ru>              1595608723 +0900  2) sv
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  3) sv1
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  4) sv1
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  5) sv11
"
)
                )
                .AtLine(2)
                .SubDeletion(0, 1000)
                .Counted()
            );
        }

        [Fact]
        public void DoesNotIgnoreLinesThatStartWithAlphanumericalChar()
        {
            Assert.Equal(
                0u,
                new GitProcessBlames(
                    new FakeProcess(
@"^7f30cd2 (<svikk@live.ru>              1595608723 +0900  1) sv
^7f30cd2 (<svikk@live.ru>              1595608723 +0900  2) sv
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  3) sv1
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  4) sv1
661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  5) sv11
"
)
                )
                .AtLine(2)
                .SubDeletion(0, 1000)
                .Ignored()
            );
        }

        [Fact]
        public void HandlesSingleLine()
        {
            Assert.Equal(
                "viktor_semenov@outlook.com",
                new GitProcessBlames(
                    new FakeProcess(
@"661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  1) sv1
"
)
                )
                .AtLine(0)
                .SubDeletion(0, 1000)
                .Email()
            );
        }

        [Fact]
        public void ExpectsEmptyLineLast()
        {
            Assert.Throws<System.InvalidOperationException>(
                () => 
                new GitProcessBlames(
                    new FakeProcess("661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  1) sv1")
                )
                .AtLine(0)
            );
        }

        [Fact]
        public void ParsesEmail()
        {
            Assert.Equal(
                "viktor_semenov@outlook.com",
                new GitProcessBlames(
                    new FakeProcess(
@"661ab997 (<viktor_semenov@outlook.com> 1603634378 +0900  1) sv1
"
)
                )
                .AtLine(0)
                .SubDeletion(0, 1000)
                .Email()
            );
        }
    }
}