using NUnit.Framework;

namespace DevRating.Rating.Test
{
    public sealed class EloPointsFormulaTests
    {
        [Test]
        public void IncreaseHigherRatedPlayerPoints()
        {
            var elo = new EloPointsFormula(32, 400);

            var points = elo.UpdatedPoints(1, 2400, 2000);
            
            Assert.AreEqual(2403, points,0.1d);
        }
        
        [Test]
        public void DecreaseLowerRatedPlayerPoints()
        {
            var elo = new EloPointsFormula(32, 400);

            var points = elo.UpdatedPoints(0, 2000, 2400);
            
            Assert.AreEqual(1997, points,0.1d);
        }
        
        [Test]
        public void DecreaseHigherRatedPlayerPoints()
        {
            var elo = new EloPointsFormula(32, 400);

            var points = elo.UpdatedPoints(0, 2400, 2000);
            
            Assert.AreEqual(2371, points,0.1d);
        }
        
        [Test]
        public void IncreaseLowerRatedPlayerPoints()
        {
            var elo = new EloPointsFormula(32, 400);

            var points = elo.UpdatedPoints(1, 2000, 2400);
            
            Assert.AreEqual(2029, points,0.1d);
        }
    }
}