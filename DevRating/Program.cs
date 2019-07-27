using DevRating.VersionControlSystem.Git;

namespace DevRating
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            new Git(new Process(new WorkingDirectory(args)))
                .Rating()
                .PrintToConsole();
        }
    }
}