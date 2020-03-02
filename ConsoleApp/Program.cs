using System;
using System.Linq;
using DevRating.DefaultObject;
using DevRating.Domain;
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
            var organization = "Current organization";

            if (!args.Any())
            {
                PrintUsage(console);
            }
            else if (args[0].Equals("show", StringComparison.OrdinalIgnoreCase))
            {
                using var repository = new Repository(args[1]);

                Application().PrintTo(
                    console,
                    Diff(
                        organization,
                        args[2],
                        args[3],
                        new LibGit2LastMajorUpdateTag(repository).Sha(),
                        repository
                    )
                );
            }
            else if (args[0].Equals("add", StringComparison.OrdinalIgnoreCase))
            {
                using var repository = new Repository(args[1]);

                Application().Save(
                    Diff(
                        organization,
                        args[2],
                        args[3],
                        new LibGit2LastMajorUpdateTag(repository).Sha(),
                        repository
                    )
                );
            }
            else if (args[0].Equals("top", StringComparison.OrdinalIgnoreCase))
            {
                Application().Top(console, organization);
            }
            else
            {
                PrintUsage(console);
            }
        }

        private static LibGit2Diff Diff(
            string organization,
            string start,
            string end,
            Envelope since,
            IRepository repository
        )
        {
            return new LibGit2Diff(
                start,
                end,
                since,
                repository,
                repository.Network.Remotes.First().Url,
                new DefaultEnvelope(),
                organization
            );
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