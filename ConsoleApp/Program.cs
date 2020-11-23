// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
            var output = new StandardOutput();
            var organization = "Current organization";

            if (!args.Any())
            {
                PrintUsage(output);
            }
            else if (args[0].Equals("show", StringComparison.OrdinalIgnoreCase))
            {
                using var repository = new Repository(args[1]);
                var before = args.Length == 4 ? args[2] : args[2] + "~";
                var after = args.Length == 4 ? args[3] : args[2];
                var diff = new LibGit2Diff(
                    before,
                    after,
                    new LibGit2LastMajorUpdateTag(repository, before).Sha(),
                    repository,
                    repository.Network.Remotes.First().Url,
                    new DefaultEnvelope(),
                    organization
                );

                Application().PrintTo(output, diff);
            }
            else if (args[0].Equals("add", StringComparison.OrdinalIgnoreCase))
            {
                using var repository = new Repository(args[1]);
                var before = args.Length == 4 || args.Length == 6 ? args[2] : args[2] + "~";
                var after = args.Length == 4 || args.Length == 6 ? args[3] : args[2];
                var link = args.Length == 5 || args.Length == 6 ? new DefaultEnvelope(args.Last()) : new DefaultEnvelope();
                var diff = new LibGit2Diff(
                    before,
                    after,
                    new LibGit2LastMajorUpdateTag(repository, before).Sha(),
                    repository,
                    repository.Network.Remotes.First().Url,
                    link,
                    organization
                );

                var app = Application();

                app.Save(diff);

                app.PrintTo(output, diff);
            }
            else if (args[0].Equals("top", StringComparison.OrdinalIgnoreCase))
            {
                Application().Top(output, organization);
            }
            else
            {
                PrintUsage(output);
            }
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

        private static void PrintUsage(Output output)
        {
            output.WriteLine("Dev Rating evaluates developers rating and rewards based on git diff.");
            output.WriteLine();
            output.WriteLine("Usage:");
            output.WriteLine("  devrating top");
            output.WriteLine("  devrating show <path> (<base> <head> | <merge>)");
            output.WriteLine("  devrating add <path> (<base> <head> | <merge>) [-l <link>]");
            output.WriteLine();
            output.WriteLine("Description:");
            output.WriteLine("  top       Print the rating");
            output.WriteLine("  show      Print a saved reward from the local DB");
            output.WriteLine("  add       Insert a reward into the local DB");
            output.WriteLine("  <path>    Path to a local repository. E.g. '~/repos/devrating'");
            output.WriteLine("  <base>    The parent commit of the first commit of a PR");
            output.WriteLine("  <head>    The last commit of a PR");
            output.WriteLine("  <merge>   A merge or squash commit of a merged PR");
            output.WriteLine("  <link>    A link to a PR, issue or so");
        }
    }
}