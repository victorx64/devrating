using System.CommandLine;
using System.CommandLine.Invocation;
using devrating.factory;
using devrating.git;
using devrating.sqlite;
using Microsoft.Data.Sqlite;

namespace devrating.consoleapp;

internal static class Program
{
    private static void Main(string[] args)
    {
        var output = new StandardOutput();
        var addCommand = new Command("add", "Update the rating");
        var addMergeCommand = new Command("commit", "Update the rating analyzing a merge commit");
        addMergeCommand.AddOption(new Option<DirectoryInfo>(new[] { "--path", "-p" }, "Path to a local repository. E.g. '~/repos/devrating'") { IsRequired = true }.ExistingOnly());
        addMergeCommand.AddOption(new Option<string>(new[] { "--merge", "-m" }, "A merge commit. Takes diff of <merge>~ and <merge>") { IsRequired = true });
        addMergeCommand.AddOption(new Option<string>(new[] { "--branch", "-r" }, "The main branch name. 'main' by default"));
        addMergeCommand.AddOption(new Option<string>(new[] { "--email", "-a" }, "Email of the PR author"));
        addMergeCommand.AddOption(new Option<string>(new[] { "--link", "-l" }, "A link to the PR, issue or so"));
        addMergeCommand.AddOption(new Option<string>(new[] { "--org", "-o" }, "Name of the repository owner"));
        addMergeCommand.AddOption(new Option<string>(new[] { "--name", "-n" }, "Name of the repository"));
        addMergeCommand.AddOption(new Option<DateTimeOffset>(new[] { "--time", "-t" }, "A moment when the PR was merged"));
        addMergeCommand.Handler = CommandHandler.Create<DirectoryInfo, string, string?, string?, string?, string?, string?, DateTimeOffset?>(
            (path, merge, branch, email, link, org, name, time) =>
            {
                var first = merge + "~";
                var second = merge;
                var diff = new GitProcessDiff(
                    first,
                    second,
                    new GitProcessLastMajorUpdateTag(path.FullName, first).Sha(),
                    path.FullName,
                    branch ?? "main",
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

        var addDiffCommand = new Command("diff", "Update the rating analyzing diff");
        addDiffCommand.AddOption(new Option<DirectoryInfo>(new[] { "--path", "-p" }, "Path to a local repository. E.g. '~/repos/devrating'") { IsRequired = true }.ExistingOnly());
        addDiffCommand.AddOption(new Option<string>(new[] { "--base", "-b" }, "The first commit of diff") { IsRequired = true });
        addDiffCommand.AddOption(new Option<string>(new[] { "--head", "-e" }, "The second commit of diff") { IsRequired = true });
        addDiffCommand.AddOption(new Option<string>(new[] { "--branch", "-r" }, "The main branch name. 'main' by default"));
        addDiffCommand.AddOption(new Option<string>(new[] { "--email", "-a" }, "Email of the PR author"));
        addDiffCommand.AddOption(new Option<string>(new[] { "--link", "-l" }, "A link to the PR, issue or so"));
        addDiffCommand.AddOption(new Option<string>(new[] { "--org", "-o" }, "Name of the repository owner"));
        addDiffCommand.AddOption(new Option<string>(new[] { "--name", "-n" }, "Name of the repository"));
        addDiffCommand.AddOption(new Option<DateTimeOffset>(new[] { "--time", "-t" }, "A moment when the PR was merged"));
        addDiffCommand.Handler = CommandHandler.Create<DirectoryInfo, string, string, string?, string?, string?, string?, string?, DateTimeOffset?>(
            (path, @base, head, branch, email, link, org, name, time) =>
            {
                var diff = new GitProcessDiff(
                    @base,
                    head,
                    new GitProcessLastMajorUpdateTag(path.FullName, @base).Sha(),
                    path.FullName,
                    branch ?? "main",
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
        serializeMergeCommand.AddOption(new Option<string>(new[] { "--branch", "-r" }, "The main branch name. 'main' by default"));
        serializeMergeCommand.AddOption(new Option<string>(new[] { "--email", "-a" }, "Email of the PR author"));
        serializeMergeCommand.AddOption(new Option<string>(new[] { "--link", "-l" }, "A link to the PR, issue or so"));
        serializeMergeCommand.AddOption(new Option<string>(new[] { "--org", "-o" }, "Name of the repository owner"));
        serializeMergeCommand.AddOption(new Option<string>(new[] { "--name", "-n" }, "Name of the repository"));
        serializeMergeCommand.AddOption(new Option<DateTimeOffset>(new[] { "--time", "-t" }, "A moment when the PR was merged"));
        serializeMergeCommand.Handler = CommandHandler.Create<DirectoryInfo, string, string?, string?, string?, string?, string?, DateTimeOffset?>(
            (path, merge, branch, email, link, org, name, time) =>
            {
                var first = merge + "~";
                var second = merge;

                output.WriteLine(
                    new GitProcessDiff(
                        first,
                        second,
                        new GitProcessLastMajorUpdateTag(path.FullName, first).Sha(),
                        path.FullName,
                        branch ?? "main",
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
        serializeDiffCommand.AddOption(new Option<string>(new[] { "--branch", "-r" }, "The main branch name. 'main' by default"));
        serializeDiffCommand.AddOption(new Option<string>(new[] { "--email", "-a" }, "Email of the PR author"));
        serializeDiffCommand.AddOption(new Option<string>(new[] { "--link", "-l" }, "A link to the PR, issue or so"));
        serializeDiffCommand.AddOption(new Option<string>(new[] { "--org", "-o" }, "Name of the repository owner"));
        serializeDiffCommand.AddOption(new Option<string>(new[] { "--name", "-n" }, "Name of the repository"));
        serializeDiffCommand.AddOption(new Option<DateTimeOffset>(new[] { "--time", "-t" }, "A moment when the PR was merged"));
        serializeDiffCommand.Handler = CommandHandler.Create<DirectoryInfo, string, string, string?, string?, string?, string?, string?, DateTimeOffset?>(
            (path, @base, head, branch, email, link, org, name, time) =>
            {
                output.WriteLine(
                    new GitProcessDiff(
                        @base,
                        head,
                        new GitProcessLastMajorUpdateTag(path.FullName, @base).Sha(),
                        path.FullName,
                        branch ?? "main",
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

        var showCommand = new Command("show", "Print previously added diff details");
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

        var applyCommand = new Command("apply", "Deserialize diff metadata and update the rating");
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

        var topCommand = new Command("top", "Print the rating");
        topCommand.AddOption(new Option<string>(new[] { "--org", "-o" }, "Name of the repository owner"));
        topCommand.AddOption(new Option<string>(new[] { "--name", "-n" }, "Name of the repository"));
        topCommand.Handler = CommandHandler.Create<string?, string?>(
            (org, name) =>
            {
                Application().Top(output, org ?? "none", name ?? "unnamed");
            }
        );

        var rootCommand = new RootCommand(
            "Dev Rating suggests the optimal Pull Request size for each contributor " + 
            "so that the PRs will have the same expected durability of the added lines of code."
        );
        rootCommand.AddCommand(addCommand);
        rootCommand.AddCommand(showCommand);
        rootCommand.AddCommand(serializeCommand);
        rootCommand.AddCommand(applyCommand);
        rootCommand.AddCommand(topCommand);
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
            new DefaultFormula()
        );
    }
}
