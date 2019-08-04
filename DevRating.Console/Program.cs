using System.Collections.Generic;
using DevRating.Rating;

namespace DevRating.Console
{
    internal static class Program
    {
        private static void Main()
        {
            new Git.Git(new List<IPlayer>())
                .Developers();
        }
    }
}