using System.Threading.Tasks;
using DevRating.Game;
using DevRating.Rating;

namespace DevRating.Console
{
    internal static class Program
    {
        private static async Task Main()
        {
            await new Git.Git(
                    ".",
                    "HEAD~3",
                    "HEAD")
                .WriteInto(
                    new GamesLog(
                        new DictionaryPlayers(
                            new DefaultPlayer(
                                new DefaultGame(1200d))),
                        new EloPointsFormula(),
                        2000d));
        }
    }
}