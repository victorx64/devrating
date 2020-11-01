// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

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
            var output = new StandardOutput();
            var organization = "Current organization";

            if (!args.Any())
            {
                PrintUsage(output);
            }
            else if (args[0].Equals("show", StringComparison.OrdinalIgnoreCase))
            {
                using var repository = new Repository(args[1]);

                Application().PrintTo(
                    output,
                    Diff(
                        organization,
                        args[2],
                        args[3],
                        new LibGit2LastMajorUpdateTag(repository, args[2]).Sha(),
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
                        new LibGit2LastMajorUpdateTag(repository, args[2]).Sha(),
                        repository
                    )
                );
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

        private static void PrintUsage(Output output)
        {
            output.WriteLine("Dev Rating evaluates developers rating and rewards based on git diff.");
            output.WriteLine();
            output.WriteLine("Usage:");
            output.WriteLine("  devrating top");
            output.WriteLine("  devrating show <path> <before> <after>");
            output.WriteLine("  devrating add <path> <before> <after>");
            output.WriteLine();
            output.WriteLine("Description:");
            output.WriteLine("  top        Print the rating");
            output.WriteLine("  show       Print a reward for the work between commits");
            output.WriteLine("  add        Update the rating by committing the work between commits");
            output.WriteLine("  <path>     Path to a local repository. E.g. '~/repos/devrating'");
            output.WriteLine("  <before>   Sha of the parent commit of the first commit of the work");
            output.WriteLine("  <after>    Sha of the last commit of the work");
        }
    }
}