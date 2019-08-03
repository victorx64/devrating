namespace DevRating.Console
{
    internal static class Program
    {
        private static void Main()
        {
            new Git(new Players())
                .Players();
        }
    }
}