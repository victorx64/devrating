using System.Threading.Tasks;
using DevRating.Rating;

namespace DevRating.Console
{
    internal static class Program
    {
        private static async Task Main()
        {
            var git = new Git.Git(
                ".",
                "151bdb5ebfd0cfbcad0aa10d6327ff79e534fda5",
                "HEAD");

            var players = new DictionaryPlayers();

            var log = new GamesLog(
                players: players,
                @default: new DefaultPlayer(new Game("default", 1200d)),
                entropy: new DefaultPlayer(new Game("entropy", 1200d)), 
                formula: new EloPointsFormula());

            await git.WriteInto(log);

            players.PrintToConsole();
        }
    }
}