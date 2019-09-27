using NUnit.Framework;

namespace DevRating.Rating.Test
{
    public class EloFormulaTests
    {
        [Test]
        public void LowPointsOnHigherWinner()
        {
            var elo = new EloFormula(30, 400);

            var points = elo.WinnerExtraPoints(1200, 1000);

            Assert.AreEqual(7.2d, points, 0.01d);
        }
        
        [Test]
        public void HighPointsOnLowerWinner()
        {
            var elo = new EloFormula(30, 400);

            var points = elo.WinnerExtraPoints(1000, 1200);

            Assert.AreEqual(22.8d, points, 0.01d);
        }
        
        [Test]
        public void HighProbabilityOnHigherWinner()
        {
            var elo = new EloFormula(30, 400);

            var probability = elo.WinProbability(1200, 1000);

            Assert.AreEqual(0.76d, probability, 0.01d);
        }
        
        [Test]
        public void LowProbabilityOnLowerWinner()
        {
            var elo = new EloFormula(30, 400);

            var probability = elo.WinProbability(1000, 1200);

            Assert.AreEqual(0.24d, probability, 0.01d);
        }
        
        [Test]
        public void HalfProbabilityOnEqualPlayers()
        {
            var elo = new EloFormula(30, 400);

            var probability = elo.WinProbability(1000, 1000);

            Assert.AreEqual(0.5d, probability, 0.01d);
        }
        
        [Test]
        public void SumOfProbabilitiesIsOne()
        {
            var elo = new EloFormula(30, 400);

            var probability1 = elo.WinProbability(1200, 1000);
            var probability2 = elo.WinProbability(1000, 1200);

            Assert.AreEqual(1.0d, probability1 + probability2, 0.01d);
        }
    }
}