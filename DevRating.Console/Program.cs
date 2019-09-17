using System.Threading.Tasks;
using DevRating.Game;
using DevRating.Rating;

namespace DevRating.Console
{
    internal static class Program
    {
        private static async Task Main()
        {
            var git = new Git.Git(
                ".",
                "HEAD~3",
                "HEAD");

            var players = new DictionaryPlayers();

            var log = new GamesLog(
                players: players,
                @default: new DefaultPlayer(new DefaultGame(1200d)),
                emptiness: new DefaultPlayer(new DefaultGame(2000d)),
                formula: new EloPointsFormula());

            await git.WriteInto(log);

            players.PrintToConsole();
        }
    }
}