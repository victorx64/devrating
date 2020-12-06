// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Linq;
using DevRating.DefaultObject;
using DevRating.EloRating;
using DevRating.GitProcessClient;
using DevRating.SqliteClient;
using Microsoft.Data.Sqlite;

namespace DevRating.ConsoleApp
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var output = new StandardOutput();
            var organization = "Current organization";
            var key = "Stub key";

            if (!args.Any())
            {
                PrintUsage(output);
            }
            else if (args[0].Equals("show", StringComparison.OrdinalIgnoreCase))
            {
                Application().PrintTo(output, new KeyDiff(args[1], args[2], key));
            }
            else if (args[0].Equals("add", StringComparison.OrdinalIgnoreCase))
            {
                var repository = args[1];
                var before = args.Length == 4 || args.Length == 6 ? args[2] : args[2] + "~";
                var after = args.Length == 4 || args.Length == 6 ? args[3] : args[2];
                var link = args.Length == 5 || args.Length == 6 ? args.Last() : null;
                var diff = new GitProcessDiff(
                    before,
                    after,
                    new GitProcessLastMajorUpdateTag(repository, before).Sha(),
                    repository,
                    key,
                    link,
                    organization
                );

                var app = Application();

                app.Save(diff);

                app.PrintTo(output, diff);
            }
            else if (args[0].Equals("serialize", StringComparison.OrdinalIgnoreCase))
            {
                var repository = args[1];
                var before = args.Length == 4 || args.Length == 6 ? args[2] : args[2] + "~";
                var after = args.Length == 4 || args.Length == 6 ? args[3] : args[2];
                var link = args.Length == 5 || args.Length == 6 ? args.Last() : null;

                output.WriteLine(
                    new GitProcessDiff(
                        before,
                        after,
                        new GitProcessLastMajorUpdateTag(repository, before).Sha(),
                        repository,
                        key,
                        link,
                        organization
                    )
                    .ToJson()
                );
            }
            else if (args[0].Equals("apply", StringComparison.OrdinalIgnoreCase))
            {
                var diff = new JsonDiff(args[1]);

                var app = Application();

                app.Save(diff);

                app.PrintTo(output, diff);
            }
            else if (args[0].Equals("top", StringComparison.OrdinalIgnoreCase))
            {
                Application().Top(output, organization);
            }
            else if (args[0].Equals("total", StringComparison.OrdinalIgnoreCase))
            {
                Application().Total(output, key, DateTimeOffset.UtcNow - TimeSpan.FromDays(90));
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
                        new SqliteConnection("Data Source=devrating.sqlite3")
                    )
                ),
                new EloFormula()
            );
        }

        private static void PrintUsage(Output output)
        {
            output.WriteLine("Dev Rating evaluates rewards based on git diff.");
            output.WriteLine();
            output.WriteLine("Usage:");
            output.WriteLine("  devrating add <path> (<base> <head> | <merge>) [-l <link>]");
            output.WriteLine("  devrating serialize <path> (<base> <head> | <merge>) [-l <link>]");
            output.WriteLine("  devrating apply <json>");
            output.WriteLine("  devrating show <base> <head>");
            output.WriteLine("  devrating top");
            output.WriteLine("  devrating total");
            output.WriteLine();
            output.WriteLine("Description:");
            output.WriteLine("  add         Evaluate the reward and insert it into the local DB");
            output.WriteLine("  serialize   Serialize diff metadata");
            output.WriteLine("  apply       Deserialize diff metadata, evaluate the reward and insert it into the local DB");
            output.WriteLine("  show        Print the saved reward from the local DB");
            output.WriteLine("  top         Print the rating on the stability of code");
            output.WriteLine("  total       Print the total rewards for the last 90 days");
            output.WriteLine();
            output.WriteLine("  <path>      Path to a local repository. E.g. '~/repos/devrating'");
            output.WriteLine("  <base>      The first commit of diff");
            output.WriteLine("  <head>      The second commit of diff");
            output.WriteLine("  <merge>     A merge commit. Takes diff of <merge>~ and <merge>");
            output.WriteLine("  <link>      A link to a PR, issue or so");
            output.WriteLine("  <json>      Serialized diff metadata");
        }
    }
}