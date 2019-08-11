using DevRating.Rating;

namespace DevRating.Console
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var arguments = new DefaultArguments(args);

            new Report(
                    new Git.Git(
                        new DefaultPlayer(
                            new Elo()),
                        arguments.OldestCommit(),
                        arguments.NewestCommit()),
                    arguments.Verbose()
                        ? (Output) new VerboseConsoleOutput()
                        : (Output) new QuiteConsoleOutput())
                .Print();
        }
    }
}