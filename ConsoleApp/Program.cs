using System;
using System.Threading.Tasks;
using DevRating.Domain.Git;
using DevRating.Domain.RatingSystem;
using DevRating.LibGit2SharpClient;
using DevRating.SqlClient;

namespace DevRating.ConsoleApp
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            var modifications = new DefaultModificationsCollection();

            var repository = new LibGit2Repository(args[0]);

            foreach (var commit in repository.Commits(args[1], args[2]))
            {
                await commit.WriteInto(modifications);
            }

            var text = modifications.PutTo(new SqlModificationsStorage(args[3], new EloFormula()));

            Console.WriteLine(text);
        }
    }
}