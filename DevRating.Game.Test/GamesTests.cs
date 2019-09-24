using System.Linq;
using System.Threading.Tasks;
using DevRating.Rating;
using NUnit.Framework;

namespace DevRating.Game.Test
{
    public class GamesTests
    {
        [Test]
        public async Task ReturnSingleMatchOnSingleAddition()
        {
            var matches = new FakeMatches(1200d);
            
            var games = new Games("sha", "author", new EloFormula(2d, 400d), 2000);

            games.AddAdditions(1);

            await games.PushInto(matches);

            Assert.AreEqual(1, (await matches.Matches("author")).Count());
        }
        
        [Test]
        public async Task ReturnSingleMatchOnMultipleAdditions()
        {
            var matches = new FakeMatches(1200d);
            
            var games = new Games("sha", "author", new EloFormula(2d, 400d), 2000);

            games.AddAdditions(1);
            games.AddAdditions(1);

            await games.PushInto(matches);

            Assert.AreEqual(1, (await matches.Matches("author")).Count());
        }
        
        [Test]
        public async Task ReturnSingleMatchOnSingleDeletion()
        {
            var matches = new FakeMatches(1200d);
            
            var games = new Games("sha", "author", new EloFormula(2d, 400d), 2000);

            games.AddDeletion("victim");

            await games.PushInto(matches);

            Assert.AreEqual(1, (await matches.Matches("author")).Count());
            Assert.AreEqual(1, (await matches.Matches("victim")).Count());
            Assert.AreEqual(1, (await matches.Matches("author")).First().Rounds());
            Assert.AreEqual(1, (await matches.Matches("victim")).First().Rounds());
        }
        
        [Test]
        public async Task ReturnSingleMatchOnMultipleDeletions()
        {
            var matches = new FakeMatches(1200d);
            
            var games = new Games("sha", "author", new EloFormula(2d, 400d), 2000);

            games.AddDeletion("victim");
            games.AddDeletion("victim");

            await games.PushInto(matches);

            Assert.AreEqual(1, (await matches.Matches("author")).Count());
            Assert.AreEqual(1, (await matches.Matches("victim")).Count());
            Assert.AreEqual(2, (await matches.Matches("author")).First().Rounds());
            Assert.AreEqual(2, (await matches.Matches("victim")).First().Rounds());
        }
    }
}