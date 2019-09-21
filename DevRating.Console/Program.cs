using System.Threading.Tasks;
using DevRating.AzureTable;
using DevRating.Game;
using DevRating.Git;
using DevRating.Rating;

namespace DevRating.Console
{
    internal static class Program
    {
        private static async Task Main()
        {
            var history = (GamesHistory) await new GitRepository(".", "HEAD")
                .History(new GamesHistoryFactory(new EloFormula(), 2000d));

            await history.PushInto(new AzureMatches("","key","match"));
        }
    }
}