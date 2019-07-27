using System;
using DevRating.VersionControlSystem.Git;

namespace DevRating
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var rating = new Git(new Process())
                .UpdatedRating(new Rating.Elo.Rating());
            
            rating.PrintToConsole();
        }
    }
}
