// Copyright (c) 2019-present Viktor Semenov
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using DevRating.DefaultObject;
using DevRating.EloRating;
using DevRating.GitProcessClient;
using DevRating.SqliteClient;
using Microsoft.Data.Sqlite;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;

namespace DevRating.ConsoleApp
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var output = new StandardOutput();
            var addCommand = new Command("add", "Evaluate the reward and insert it into the local DB");
            var addMergeCommand = new Command("commit", "Evaluate the reward for a merge commit and insert it into the local DB");
            addMergeCommand.AddOption(new Option<DirectoryInfo>(new[] { "--path", "-p" }, "Path to a local repository. E.g. '~/repos/devrating'") { IsRequired = true }.ExistingOnly());
            addMergeCommand.AddOption(new Option<string>(new[] { "--merge", "-m" }, "A merge commit. Takes diff of <merge>~ and <merge>") { IsRequired = true });
            addMergeCommand.AddOption(new Option<string>(new[] { "--email", "-a" }, "Email of the PR author"));
            addMergeCommand.AddOption(new Option<string>(new[] { "--link", "-l" }, "A link to the PR, issue or so"));
            addMergeCommand.AddOption(new Option<string>(new[] { "--org", "-o" }, "Name of the repository owner"));
            addMergeCommand.AddOption(new Option<string>(new[] { "--name", "-n" }, "Name of the repository"));
            addMergeCommand.AddOption(new Option<DateTimeOffset>(new[] { "--time", "-t" }, "A moment when the PR was merged"));
            addMergeCommand.Handler = CommandHandler.Create<DirectoryInfo, string, string?, string?, string?, string?, DateTimeOffset?>(
                (path, merge, email, link, org, name, time) =>
                {
                    var first = merge + "~";
                    var second = merge;
                    var diff = new GitProcessDiff(
                        first,
                        second,
                        new GitProcessLastMajorUpdateTag(path.FullName, first).Sha(),
                        path.FullName,
                        name ?? "unnamed",
                        link,
                        org ?? "none",
                        time ?? DateTimeOffset.UtcNow,
                        email
                    );

                    var app = Application();

                    app.Save(diff);

                    app.PrintTo(output, diff);
                }
            );
            addCommand.AddCommand(addMergeCommand);

            var addDiffCommand = new Command("diff", "Evaluate the reward for diff and insert it into the local DB");
            addDiffCommand.AddOption(new Option<DirectoryInfo>(new[] { "--path", "-p" }, "Path to a local repository. E.g. '~/repos/devrating'") { IsRequired = true }.ExistingOnly());
            addDiffCommand.AddOption(new Option<string>(new[] { "--base", "-b" }, "The first commit of diff") { IsRequired = true });
            addDiffCommand.AddOption(new Option<string>(new[] { "--head", "-e" }, "The second commit of diff") { IsRequired = true });
            addDiffCommand.AddOption(new Option<string>(new[] { "--email", "-a" }, "Email of the PR author"));
            addDiffCommand.AddOption(new Option<string>(new[] { "--link", "-l" }, "A link to the PR, issue or so"));
            addDiffCommand.AddOption(new Option<string>(new[] { "--org", "-o" }, "Name of the repository owner"));
            addDiffCommand.AddOption(new Option<string>(new[] { "--name", "-n" }, "Name of the repository"));
            addDiffCommand.AddOption(new Option<DateTimeOffset>(new[] { "--time", "-t" }, "A moment when the PR was merged"));
            addDiffCommand.Handler = CommandHandler.Create<DirectoryInfo, string, string, string?, string?, string?, string?, DateTimeOffset?>(
                (path, @base, head, email, link, org, name, time) =>
                {
                    var diff = new GitProcessDiff(
                        @base,
                        head,
                        new GitProcessLastMajorUpdateTag(path.FullName, @base).Sha(),
                        path.FullName,
                        name ?? "unnamed",
                        link,
                        org ?? "none",
                        time ?? DateTimeOffset.UtcNow,
                        email
                    );

                    var app = Application();

                    app.Save(diff);

                    app.PrintTo(output, diff);
                }
            );
            addCommand.AddCommand(addDiffCommand);

            var serializeCommand = new Command("serialize", "Serialize diff metadata");
            var serializeMergeCommand = new Command("commit", "Serialize merge commit metadata");
            serializeMergeCommand.AddOption(new Option<DirectoryInfo>(new[] { "--path", "-p" }, "Path to a local repository. E.g. '~/repos/devrating'") { IsRequired = true }.ExistingOnly());
            serializeMergeCommand.AddOption(new Option<string>(new[] { "--merge", "-m" }, "A merge commit. Takes diff of <merge>~ and <merge>") { IsRequired = true });
            serializeMergeCommand.AddOption(new Option<string>(new[] { "--email", "-a" }, "Email of the PR author"));
            serializeMergeCommand.AddOption(new Option<string>(new[] { "--link", "-l" }, "A link to the PR, issue or so"));
            serializeMergeCommand.AddOption(new Option<string>(new[] { "--org", "-o" }, "Name of the repository owner"));
            serializeMergeCommand.AddOption(new Option<string>(new[] { "--name", "-n" }, "Name of the repository"));
            serializeMergeCommand.AddOption(new Option<DateTimeOffset>(new[] { "--time", "-t" }, "A moment when the PR was merged"));
            serializeMergeCommand.Handler = CommandHandler.Create<DirectoryInfo, string, string?, string?, string?, string?, DateTimeOffset?>(
                (path, merge, email, link, org, name, time) =>
                {
                    var first = merge + "~";
                    var second = merge;

                    output.WriteLine(
                        new GitProcessDiff(
                            first,
                            second,
                            new GitProcessLastMajorUpdateTag(path.FullName, first).Sha(),
                            path.FullName,
                            name ?? "unnamed",
                            link,
                            org ?? "none",
                            time ?? DateTimeOffset.UtcNow,
                            email
                        )
                        .ToJson()
                    );
                }
            );
            serializeCommand.AddCommand(serializeMergeCommand);

            var serializeDiffCommand = new Command("diff", "Serialize diff metadata");
            serializeDiffCommand.AddOption(new Option<DirectoryInfo>(new[] { "--path", "-p" }, "Path to a local repository. E.g. '~/repos/devrating'") { IsRequired = true }.ExistingOnly());
            serializeDiffCommand.AddOption(new Option<string>(new[] { "--base", "-b" }, "The first commit of diff") { IsRequired = true });
            serializeDiffCommand.AddOption(new Option<string>(new[] { "--head", "-e" }, "The second commit of diff") { IsRequired = true });
            serializeDiffCommand.AddOption(new Option<string>(new[] { "--email", "-a" }, "Email of the PR author"));
            serializeDiffCommand.AddOption(new Option<string>(new[] { "--link", "-l" }, "A link to the PR, issue or so"));
            serializeDiffCommand.AddOption(new Option<string>(new[] { "--org", "-o" }, "Name of the repository owner"));
            serializeDiffCommand.AddOption(new Option<string>(new[] { "--name", "-n" }, "Name of the repository"));
            serializeDiffCommand.AddOption(new Option<DateTimeOffset>(new[] { "--time", "-t" }, "A moment when the PR was merged"));
            serializeDiffCommand.Handler = CommandHandler.Create<DirectoryInfo, string, string, string?, string?, string?, string?, DateTimeOffset?>(
                (path, @base, head, email, link, org, name, time) =>
                {
                    output.WriteLine(
                        new GitProcessDiff(
                            @base,
                            head,
                            new GitProcessLastMajorUpdateTag(path.FullName, @base).Sha(),
                            path.FullName,
                            name ?? "unnamed",
                            link,
                            org ?? "none",
                            time ?? DateTimeOffset.UtcNow,
                            email
                        )
                        .ToJson()
                    );
                }
            );
            serializeCommand.AddCommand(serializeDiffCommand);

            var showCommand = new Command("show", "Print the saved reward from the local DB");
            showCommand.AddOption(new Option<string>(new[] { "--base", "-b" }, "The first commit of diff") { IsRequired = true });
            showCommand.AddOption(new Option<string>(new[] { "--head", "-e" }, "The second commit of diff") { IsRequired = true });
            showCommand.AddOption(new Option<string>(new[] { "--org", "-o" }, "Name of the repository owner"));
            showCommand.AddOption(new Option<string>(new[] { "--name", "-n" }, "Name of the repository"));
            showCommand.Handler = CommandHandler.Create<string, string, string?, string?>(
                (@base, head, org, name) =>
                {
                    Application().PrintTo(output, new ThinDiff(org ?? "none", name ?? "unnamed", @base, head));
                }
            );

            var applyCommand = new Command("apply", "Deserialize diff metadata, evaluate the reward and insert it into the local DB");
            applyCommand.AddOption(new Option<string>(new[] { "--json", "-j" }, "Serialized diff metadata") { IsRequired = true });
            applyCommand.Handler = CommandHandler.Create<string>(
                (json) =>
                {
                    var diff = new JsonDiff(json);

                    var app = Application();

                    app.Save(diff);

                    app.PrintTo(output, diff);
                }
            );

            var topCommand = new Command("top", "Print the rating on the stability of code");
            topCommand.AddOption(new Option<string>(new[] { "--org", "-o" }, "Name of the repository owner"));
            topCommand.AddOption(new Option<string>(new[] { "--name", "-n" }, "Name of the repository"));
            topCommand.Handler = CommandHandler.Create<string?, string?>(
                (org, name) =>
                {
                    Application().Top(output, org ?? "none", name ?? "unnamed");
                }
            );

            var totalCommand = new Command("total", "Print the total rewards for the last 90 days");
            totalCommand.AddOption(new Option<string>(new[] { "--org", "-o" }, "Name of the repository owner"));
            totalCommand.AddOption(new Option<string>(new[] { "--name", "-n" }, "Name of the repository"));
            totalCommand.Handler = CommandHandler.Create<string?, string?>(
                (org, name) =>
                {
                    Application().Total(output, org ?? "none", name ?? "unnamed", DateTimeOffset.UtcNow - TimeSpan.FromDays(90));
                }
            );

            // Create a root command with some options
            var rootCommand = new RootCommand("Dev Rating evaluates rewards based on git diff.");
            rootCommand.AddCommand(addCommand);
            rootCommand.AddCommand(showCommand);
            rootCommand.AddCommand(serializeCommand);
            rootCommand.AddCommand(topCommand);
            rootCommand.AddCommand(totalCommand);
            rootCommand.Invoke(args);
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
    }
}