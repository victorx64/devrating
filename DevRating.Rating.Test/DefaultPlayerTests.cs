using NUnit.Framework;

namespace DevRating.Rating.Test
{
    public sealed class DefaultPlayerTests
    {
        [Test]
        public void IncreaseWinnerPoints()
        {
            var elo = new EloPointsFormula();
            
            var a = new DefaultPlayer(elo);
            var b = new DefaultPlayer(elo);

            var winner = a.Winner(b);
            
            Assert.Greater(winner.Points(), a.Points());
        }

        [Test]
        public void DecreaseLoserPoints()
        {
            var elo = new EloPointsFormula();
            
            var a = new DefaultPlayer(elo);
            var b = new DefaultPlayer(elo);

            var loser = a.Loser(b);
            
            Assert.Less(loser.Points(), a.Points());
        }
    }
}