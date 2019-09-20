using System.Threading.Tasks;
using DevRating.AzureTable;
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
                    "HEAD")
                .WriteInto(new GamesLog(new AzureMatches(), new EloFormula(), 2000d, "HEAD", ""));
        }
    }
}