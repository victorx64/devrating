using System.Threading.Tasks;
using DevRating.AzureTable;
using DevRating.Game;
using DevRating.Git;
using DevRating.Git.LibGit2Sharp;
using DevRating.InMemoryStorage;
using DevRating.Rating;

namespace DevRating.Console
{
    internal static class Program
    {
        private static async Task Main()
        {
            var history = (Games) await new Commit(new LibGit2Repository("."), "HEAD")
                .Modifications(new GamesFactory(new EloFormula(), 2000d));

//            await history.PushInto(new AzureMatches("","key","match"));

            var matches = new InMemoryMatches(1200d);

            await history.PushInto(matches);
            
            matches.PrintToConsole();
        }
    }
}