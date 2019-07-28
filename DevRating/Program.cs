using DevRating.VersionControlSystem.Git;

namespace DevRating
{
    internal static class Program
    {
        private static void Main()
        {
            new Git(new Process())
                .Rating()
                .PrintToConsole();
        }
    }
}