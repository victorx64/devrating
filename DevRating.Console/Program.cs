using System.Threading.Tasks;

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

            var authors = new Players();

            await git.WriteInto(authors);

            authors.PrintToConsole();
        }
    }
}