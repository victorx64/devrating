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

            var authors = new DictionaryPlayers();

            var log = new PlayersChangeLog(authors, new SimplePlayer(), new EloPointsFormula());

            await git.WriteInto(log);

            authors.PrintToConsole();
        }
    }
}