using System.Threading.Tasks;
using DevRating.AzureTable;
using DevRating.Game;
using DevRating.Git;
using DevRating.Git.LibGit2Sharp;
using DevRating.Rating;

namespace DevRating.Console
{
    internal static class Program
    {
        private static async Task Main()
        {
            using var repository = new LibGit2Repository(".");

            var history = (Games) await new Commit(repository, "HEAD")
                .Modifications(new GamesFactory(new EloFormula(), 2000d));

//            await history.PushInto(new AzureMatches("", "key", "match", 1200d));
        }
    }
}