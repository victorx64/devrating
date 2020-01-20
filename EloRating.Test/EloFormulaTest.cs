using DevRating.EloRating.Fake;
using Xunit;

namespace DevRating.EloRating.Test
{
    public sealed class EloFormulaTest
    {
        [Fact]
        public void ReturnsDefaultRating()
        {
            Assert.True(new EloFormula().DefaultRating() > 0);
        }

        [Fact]
        public void CalculatesProperWinnerNewRating()
        {
            Assert.Equal(1207.2,
                new EloFormula(30, 400, 1100)
                    .WinnerNewRating(1200, new[] {new FakeMatch(1000, 1)}),
                1);
        }

        [Fact]
        public void CalculatesProperLoserNewRating()
        {
            Assert.Equal(1177.2,
                new EloFormula(30, 400, 1100)
                    .LoserNewRating(1200, new FakeMatch(1000, 1)),
                1);
        }

        [Fact]
        public void ReturnsPositiveRatingAfterManyLoses()
        {
            var formula = new EloFormula(2d, 400d, 1500d);
            var winner = formula.DefaultRating();
            var loser = formula.DefaultRating();
            var count = 10u;

            for (var i = 0; i < 500; ++i)
            {
                var w = winner;
                var l = loser;

                winner = formula.WinnerNewRating(w, new[] {new FakeMatch(l, count)});
                loser = formula.LoserNewRating(l, new FakeMatch(w, count));
            }

            Assert.True(loser > 0);
        }

        [Fact]
        public void ReturnsSameWinnerRatingWithIterationsAndSingleCall()
        {
            var formula = new EloFormula();
            var initialW = 1400d;
            var initialL = 1234d;
            var iterations = 500u;

            double WinnerRatingAfterIterations()
            {
                var one = 1u;
                var winner = initialW;
                var loser = initialL;

                for (var i = 0; i < iterations; ++i)
                {
                    var w = winner;
                    var l = loser;

                    winner = formula.WinnerNewRating(w, new[] {new FakeMatch(l, one)});
                    loser = formula.LoserNewRating(l, new FakeMatch(w, one));
                }

                return winner;
            }

            Assert.Equal(
                formula.WinnerNewRating(initialW, new[] {new FakeMatch(initialL, iterations)}),
                WinnerRatingAfterIterations(),
                6);
        }

        [Fact]
        public void ReturnsSameLoserRatingWithIterationsAndSingleCall()
        {
            var formula = new EloFormula();
            var initialW = 1400d;
            var initialL = 1234d;
            var iterations = 500u;

            double LoserRatingAfterIterations()
            {
                var one = 1u;
                var winner = initialW;
                var loser = initialL;

                for (var i = 0; i < iterations; ++i)
                {
                    var w = winner;
                    var l = loser;

                    winner = formula.WinnerNewRating(w, new[] {new FakeMatch(l, one)});
                    loser = formula.LoserNewRating(l, new FakeMatch(w, one));
                }

                return loser;
            }

            Assert.Equal(
                formula.LoserNewRating(initialL, new FakeMatch(initialW, iterations)),
                LoserRatingAfterIterations(),
                6);
        }

        [Fact]
        public void DoesntChangeRatingSum()
        {
            var formula = new EloFormula(2d, 400d, 1500d);
            var winner = formula.DefaultRating();
            var loser = formula.DefaultRating();
            var count = 500u;

            var w = winner;
            var l = loser;

            winner = formula.WinnerNewRating(w, new[] {new FakeMatch(l, count)});
            loser = formula.LoserNewRating(l, new FakeMatch(w, count));

            Assert.Equal(w + l, winner + loser, 5);
        }

        [Fact]
        public void DoesntChangeRatingSumOfThree()
        {
            var formula = new EloFormula(2d, 400d, 1500d);
            var first = formula.DefaultRating();
            var second = formula.DefaultRating();
            var three = formula.DefaultRating();

            var f = first;
            var s = second;
            var t = three;

            first = formula.WinnerNewRating(f, new[]
            {
                new FakeMatch(s, 110),
                new FakeMatch(t, 19),
            });
            second = formula.LoserNewRating(s, new FakeMatch(f, 110));
            three = formula.LoserNewRating(t, new FakeMatch(f, 19));

            Assert.Equal(f + s + t, first + second + three, 5);
        }

        [Fact]
        public void CalculatesProperWinProbabilityOfWinner()
        {
            Assert.Equal(0.09d, new EloFormula(1d, 400d, 1200d).WinProbabilityOfA(1400, 1800), 2);
        }

        [Fact]
        public void CalculatesProperWinProbabilityOfLoser()
        {
            Assert.Equal(0.91d, new EloFormula(1d, 400d, 1200d).WinProbabilityOfA(1800, 1400), 2);
        }
    }
}