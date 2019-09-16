namespace DevRating.Console
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var arguments = new DefaultArguments(args);

            var git = new Git.Git(
                ".",
                arguments.OldestCommit(),
                arguments.NewestCommit());
            ;
        }
    }
}