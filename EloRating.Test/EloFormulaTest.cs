using DevRating.Domain;
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
                    .WinnerNewRating(1200, new[] {new DefaultMatch(1000, 1)}),
                1);
        }

        [Fact]
        public void CalculatesProperLoserNewRating()
        {
            Assert.Equal(1177.2,
                new EloFormula(30, 400, 1100)
                    .LoserNewRating(1200, new DefaultMatch(1000, 1)),
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

                winner = formula.WinnerNewRating(w, new[] {new DefaultMatch(l, count)});
                loser = formula.LoserNewRating(l, new DefaultMatch(w, count));
            }

            Assert.True(loser > 0);
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