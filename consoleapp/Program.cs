using System.CommandLine;
using System.CommandLine.Invocation;
using devrating.factory;
using devrating.git;
using devrating.sqlite;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

namespace devrating.consoleapp;

internal static class Program
{
    private static void Main(string[] args)
    {
        using var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddSystemdConsole();
        });

        var output = new StandardOutput();
        var updateCommand = new Command("update", "Update the rating by analyzing first-parent commits of the last 90 days");
        updateCommand.AddOption(new Option<DirectoryInfo>(new[] { "--path", "-p" }, () => new DirectoryInfo("."), "Path to a local repository. E.g. '~/repos/devrating'").ExistingOnly());
        updateCommand.AddOption(new Option<string>(new[] { "--org", "-o" }, "Name of the repository owner"));
        updateCommand.AddOption(new Option<string>(new[] { "--name", "-n" }, "Name of the repository"));
        updateCommand.AddOption(new Option<bool>(new[] { "--verbose", "-v" }, "Verbose mode"));
        updateCommand.Handler = CommandHandler.Create<DirectoryInfo, string?, string?, bool?>(
            (path, org, name, verbose) =>
            {
                var debug = verbose == true ? loggerFactory : new LoggerFactory();

                var app = Application(debug);

                var commits = new GitProcess(
                    debug,
                    "git",
                    $"rev-list HEAD --first-parent --max-age={app.PeriodStart().ToUnixTimeSeconds()} --reverse --pretty=%aI",
                    path.FullName).Output();

                for (int i = 2; i < commits.Count - 1; i += 2)
                {
                    var merge = commits[i].Split(' ')[1];

                    var createdAt = DateTime.Parse(commits[i + 1]);

                    var diff = new GitDiff(
                        debug,
                        new GitProcess(debug, "git", $"rev-parse {merge}~", path.FullName).Output().First(),
                        merge,
                        new GitLastMajorUpdateTag(debug, path.FullName, merge).Sha(),
                        path.FullName,
                        name ?? "unnamed",
                        null,
                        org ?? "none",
                        createdAt
                    );

                    if (!app.IsDiffPresent(diff))
                    {
                        app.Save(diff);

                        output.WriteLine(merge);
                    }
                }
            }
        );

        var updateByOneCommand = new Command("update-by-one", "Update the rating by analyzing a merge commit");
        updateByOneCommand.AddOption(new Option<DirectoryInfo>(new[] { "--path", "-p" }, () => new DirectoryInfo("."), "Path to a local repository. E.g. '~/repos/devrating'").ExistingOnly());
        updateByOneCommand.AddOption(new Option<string>(new[] { "--merge", "-m" }, "A merge commit") { IsRequired = true });
        updateByOneCommand.AddOption(new Option<string>(new[] { "--link", "-l" }, "A link to the PR, issue or so"));
        updateByOneCommand.AddOption(new Option<string>(new[] { "--org", "-o" }, "Name of the repository owner"));
        updateByOneCommand.AddOption(new Option<string>(new[] { "--name", "-n" }, "Name of the repository"));
        updateByOneCommand.AddOption(new Option<DateTimeOffset>(new[] { "--time", "-t" }, "A moment when the PR was merged"));
        updateByOneCommand.AddOption(new Option<bool>(new[] { "--verbose", "-v" }, "Verbose mode"));
        updateByOneCommand.Handler = CommandHandler.Create<DirectoryInfo, string, string?, string?, string?, DateTimeOffset?, bool?>(
            (path, merge, link, org, name, time, verbose) =>
            {
                var debug = verbose == true ? loggerFactory : new LoggerFactory();

                var diff = new GitDiff(
                    debug,
                    new GitProcess(debug, "git", $"rev-parse {merge}~", path.FullName).Output().First(),
                    new GitProcess(debug, "git", $"rev-parse {merge}", path.FullName).Output().First(),
                    new GitLastMajorUpdateTag(debug, path.FullName, merge).Sha(),
                    path.FullName,
                    name ?? "unnamed",
                    link,
                    org ?? "none",
                    time ?? DateTimeOffset.UtcNow
                );

                var app = Application(debug);

                app.Save(diff);

                app.Print(output, diff);
            }
        );

        var serializeCommand = new Command("serialize", "Serialize commit metadata");
        serializeCommand.AddOption(new Option<DirectoryInfo>(new[] { "--path", "-p" }, () => new DirectoryInfo("."), "Path to a local repository. E.g. '~/repos/devrating'").ExistingOnly());
        serializeCommand.AddOption(new Option<string>(new[] { "--merge", "-m" }, "A merge commit") { IsRequired = true });
        serializeCommand.AddOption(new Option<string>(new[] { "--link", "-l" }, "A link to the PR, issue or so"));
        serializeCommand.AddOption(new Option<string>(new[] { "--org", "-o" }, "Name of the repository owner"));
        serializeCommand.AddOption(new Option<string>(new[] { "--name", "-n" }, "Name of the repository"));
        serializeCommand.AddOption(new Option<DateTimeOffset>(new[] { "--time", "-t" }, "A moment when the PR was merged"));
        serializeCommand.AddOption(new Option<bool>(new[] { "--verbose", "-v" }, "Verbose mode"));
        serializeCommand.Handler = CommandHandler.Create<DirectoryInfo, string, string?, string?, string?, DateTimeOffset?, bool?>(
            (path, merge, link, org, name, time, verbose) =>
            {
                var debug = verbose == true ? loggerFactory : new LoggerFactory();

                output.WriteLine(
                    new GitDiff(
                        debug,
                        new GitProcess(debug, "git", $"rev-parse {merge}~", path.FullName).Output().First(),
                        new GitProcess(debug, "git", $"rev-parse {merge}", path.FullName).Output().First(),
                        new GitLastMajorUpdateTag(debug, path.FullName, merge).Sha(),
                        path.FullName,
                        name ?? "unnamed",
                        link,
                        org ?? "none",
                        time ?? DateTimeOffset.UtcNow
                    )
                    .ToJson()
                );
            }
        );

        var showCommand = new Command("show", "Print previously applied commit details");
        showCommand.AddOption(new Option<string>(new[] { "--merge", "-m" }, "The merge commit") { IsRequired = true });
        showCommand.AddOption(new Option<string>(new[] { "--org", "-o" }, "Name of the repository owner"));
        showCommand.AddOption(new Option<string>(new[] { "--name", "-n" }, "Name of the repository"));
        showCommand.Handler = CommandHandler.Create<string, string?, string?>(
            (head, org, name) =>
            {
                Application(new LoggerFactory()).Print(output, new ThinDiff(org ?? "none", name ?? "unnamed", head));
            }
        );

        var applyCommand = new Command("apply", "Deserialize commit metadata and update the rating");
        applyCommand.AddOption(new Option<string>(new[] { "--json", "-j" }, "Serialized commit metadata") { IsRequired = true });
        applyCommand.Handler = CommandHandler.Create<string>(
            (json) =>
            {
                var diff = new JsonDiff(json);

                var app = Application(new LoggerFactory());

                app.Save(diff);

                app.Print(output, diff);
            }
        );

        var topCommand = new Command("top", "Print the rating of active contributors for the last 90 days");
        topCommand.AddOption(new Option<string>(new[] { "--org", "-o" }, "Name of the repository owner"));
        topCommand.AddOption(new Option<string>(new[] { "--name", "-n" }, "Name of the repository"));
        topCommand.Handler = CommandHandler.Create<string?, string?>(
            (org, name) =>
            {
                Application(new LoggerFactory()).Top(output, org ?? "none", name ?? "unnamed");
            }
        );

        var rootCommand = new RootCommand(
            "Dev Rating suggests the optimal Pull Request size for each contributor " +
            "so that the PRs will have the same expected durability of the added lines of code."
        );
        rootCommand.Name = "devrating";
        rootCommand.AddCommand(updateCommand);
        rootCommand.AddCommand(updateByOneCommand);
        rootCommand.AddCommand(showCommand);
        rootCommand.AddCommand(serializeCommand);
        rootCommand.AddCommand(applyCommand);
        rootCommand.AddCommand(topCommand);
        rootCommand.Invoke(args);
    }

    private static ConsoleApplication Application(ILoggerFactory loggerFactory)
    {
        return new ConsoleApplication(
            loggerFactory,
            new SqliteDatabase(
                new TransactedDbConnection(
                    new SqliteConnection("Data Source=devrating.sqlite3")
                )
            ),
            new DefaultFormula()
        );
    }
}
