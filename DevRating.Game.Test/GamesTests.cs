using System.Linq;
using System.Threading.Tasks;
using DevRating.Rating;
using NUnit.Framework;

namespace DevRating.Game.Test
{
    public class GamesTests
    {
        [Test]
        public async Task ReturnSingleMatchAfterSingleAddition()
        {
            var matches = new FakeMatches(1200d);

            var games = new Games("sha", new EloFormula(2d, 400d), 2000);

            games.AddAdditions("author", 1);

            await games.PushInto(matches);

            Assert.AreEqual(1, (await matches.Matches("author")).Count());
        }

        [Test]
        public async Task ReturnSingleMatchAfterMultipleAdditions()
        {
            var matches = new FakeMatches(1200d);

            var games = new Games("sha", new EloFormula(2d, 400d), 2000);

            games.AddAdditions("author", 1);
            games.AddAdditions("author", 1);

            await games.PushInto(matches);

            Assert.AreEqual(1, (await matches.Matches("author")).Count());
        }

        [Test]
        public async Task ReturnSingleMatchAfterSingleDeletion()
        {
            var matches = new FakeMatches(1200d);

            var games = new Games("sha", new EloFormula(2d, 400d), 2000);

            games.AddDeletion("author", "victim");

            await games.PushInto(matches);

            Assert.AreEqual(1, (await matches.Matches("author")).Count());
            Assert.AreEqual(1, (await matches.Matches("victim")).Count());
            Assert.AreEqual(1, (await matches.Matches("author")).First().Rounds());
            Assert.AreEqual(1, (await matches.Matches("victim")).First().Rounds());
        }

        [Test]
        public async Task ReturnSingleMatchAfterMultipleDeletions()
        {
            var matches = new FakeMatches(1200d);

            var games = new Games("sha", new EloFormula(2d, 400d), 2000);

            games.AddDeletion("author", "victim");
            games.AddDeletion("author", "victim");

            await games.PushInto(matches);

            Assert.AreEqual(1, (await matches.Matches("author")).Count());
            Assert.AreEqual(1, (await matches.Matches("victim")).Count());
            Assert.AreEqual(2, (await matches.Matches("author")).First().Rounds());
            Assert.AreEqual(2, (await matches.Matches("victim")).First().Rounds());
        }

        [Test]
        public async Task ReturnMultipleMatchesAfterMultipleGames()
        {
            var matches = new FakeMatches(1200d);

            var first = new Games("sha", new EloFormula(2d, 400d), 2000);
            var second = new Games("sha", new EloFormula(2d, 400d), 2000);

            first.AddAdditions("author", 1);

            await first.PushInto(matches);

            second.AddDeletion("author", "victim");

            await second.PushInto(matches);

            Assert.AreEqual(2, (await matches.Matches("author")).Count());
        }

        [Test]
        public async Task MakeZeroSumRatingUpdateForBothPlayers()
        {
            var matches = new FakeMatches(1200d);

            var authorPointsBefore = await matches.Points("author");
            var victimPointsBefore = await matches.Points("victim");

            var games = new Games("sha", new EloFormula(2d, 400d), 2000);
            games.AddDeletion("author", "victim");
            await games.PushInto(matches);

            var authorPointsAfter = await matches.Points("author");
            var victimPointsAfter = await matches.Points("victim");

            Assert.Greater(victimPointsBefore, victimPointsAfter);
            Assert.Greater(authorPointsAfter, authorPointsBefore);
            Assert.AreEqual(victimPointsBefore - victimPointsAfter, authorPointsAfter - authorPointsBefore);
        }

        [Test]
        public async Task GiveZeroRatingForAddition()
        {
            var matches = new FakeMatches(1200d);

            var games = new Games("sha", new EloFormula(2d, 400d), 2000);
            games.AddAdditions("author", 1);
            await games.PushInto(matches);

            Assert.AreEqual(1200d, await matches.Points("author"));
        }

        [Test]
        public async Task GivePositiveRewardForAddition()
        {
            var matches = new FakeMatches(1200d);

            var games = new Games("sha", new EloFormula(2d, 400d), 2000);
            games.AddAdditions("author", 1);
            await games.PushInto(matches);

            var reward = (await matches.Matches("author")).Last().Reward();

            Assert.Positive(reward);
        }

        [Test]
        public async Task GivePositiveRewardForDeletion()
        {
            var matches = new FakeMatches(1200d);

            var games = new Games("sha", new EloFormula(2d, 400d), 2000);
            games.AddDeletion("author", "victim");
            await games.PushInto(matches);

            var reward = (await matches.Matches("author")).Last().Reward();

            Assert.Positive(reward);
        }

        [Test]
        public async Task GiveZeroRewardForDeletedVictim()
        {
            var matches = new FakeMatches(1200d);

            var games = new Games("sha", new EloFormula(2d, 400d), 2000);
            games.AddDeletion("author", "victim");
            await games.PushInto(matches);

            var reward = (await matches.Matches("victim")).Last().Reward();

            Assert.Zero(reward);
        }
    }
}