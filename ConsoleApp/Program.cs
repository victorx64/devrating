using System;
using System.Linq;
using DevRating.DefaultObject;
using DevRating.EloRating;
using DevRating.LibGit2SharpClient;
using DevRating.SqliteClient;
using LibGit2Sharp;
using Microsoft.Data.Sqlite;

namespace DevRating.ConsoleApp
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var console = new SystemConsole();
            
            if (!args.Any())
            {
                PrintUsage(console);
            }
            else if (args[0].Equals("show", StringComparison.OrdinalIgnoreCase))
            {
                Show(console, args[1], args[2], args[3]);
            }
            else if (args[0].Equals("add", StringComparison.OrdinalIgnoreCase))
            {
                Add(args[1], args[2], args[3]);
            }
            else if (args[0].Equals("top", StringComparison.OrdinalIgnoreCase))
            {
                Top(console);
            }
        }

        private static void Show(Console console, string path, string start, string end)
        {
            using var repository = new Repository(path);

            Application().PrintTo(console, Diff(start, end, repository));
        }

        private static void Add(string path, string start, string end)
        {
            using var repository = new Repository(path);

            Application().Save(Diff(start, end, repository));
        }

        private static LibGit2Diff Diff(string start, string end, IRepository repository)
        {
            return new LibGit2Diff(
                start,
                end,
                repository,
                repository.Network.Remotes.First().Url,
                new DefaultEnvelope(),
                "Current organization"
            );
        }

        private static void Top(Console console)
        {
            Application().Top(console, "Current organization");
        }

        private static ConsoleApplication Application()
        {
            return new ConsoleApplication(
                new SqliteDatabase(
                    new TransactedDbConnection(
                        new SqliteConnection("Data Source=devrating.db")
                    )
                ),
                new EloFormula()
            );
        }

        private static void PrintUsage(Console console)
        {
            console.WriteLine("Dev Rating evaluates developers rating and rewards based on git diff.");
            console.WriteLine();
            console.WriteLine("Usage:");
            console.WriteLine("  devrating top");
            console.WriteLine("  devrating show <path> <before> <after>");
            console.WriteLine("  devrating add <path> <before> <after>");
            console.WriteLine();
            console.WriteLine("Description:");
            console.WriteLine("  top        Print the rating");
            console.WriteLine("  show       Print a reward for the work between commits");
            console.WriteLine("  add        Update the rating by committing the work between commits");
            console.WriteLine("  <path>     Path to a local repository. E.g. '~/repos/devrating'");
            console.WriteLine("  <before>   Sha of the parent commit of the first commit of the work");
            console.WriteLine("  <after>    Sha of the last commit of the work");
        }
    }
}