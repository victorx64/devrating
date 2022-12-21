using System.CommandLine;
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

        var pathOption = new Option<DirectoryInfo>(
            new[] { "--path", "-p" },
            () => new DirectoryInfo("."),
            "Path to a local repository. E.g. '~/repos/devrating'")
        .ExistingOnly();
        var orgOption = new Option<string?>(new[] { "--org", "-o" }, "Name of the repository owner");
        var nameOption = new Option<string?>(new[] { "--name", "-n" }, "Name of the repository");
        var verboseOption = new Option<bool>(new[] { "--verbose", "-v" }, "Verbose mode");
        var gitPathspecArg = new Argument<string[]>("pathspec", "Sets `git diff` <path> argument");
        var commitArg = new Argument<string>("commit", "A merge commit");
        var linkOption = new Option<string?>(new[] { "--link", "-l" }, "A link to the PR, issue or so");
        var timeOption = new Option<DateTimeOffset?>(new[] { "--time", "-t" }, "A moment when the PR was merged");
        var jsonOption = new Option<string>(new[] { "--json", "-j" }, "Serialized commit metadata") { IsRequired = true };

        var output = new StandardOutput();

        var updateCommand = new Command("update", "Update the rating by analyzing first-parent commits of the last 90 days");

        updateCommand.Add(pathOption);
        updateCommand.Add(orgOption);
        updateCommand.Add(nameOption);
        updateCommand.Add(verboseOption);
        updateCommand.Add(gitPathspecArg);

        updateCommand.SetHandler((path, org, name, verbose, gitPaths) =>
            {
                var debug = verbose == true ? loggerFactory : new LoggerFactory();

                var app = Application(debug);

                var commits = new GitProcess(
                    debug,
                    "git",
                    new[] {
                        "rev-list",
                        "HEAD",
                        "--first-parent",
                        $"--max-age={app.PeriodStart().ToUnixTimeSeconds()}",
                        "--reverse",
                    },
                    path.FullName).Output();

                org ??= "none";
                name ??= "unnamed";

                for (int i = 1; i < commits.Count - 1; i++)
                {
                    var merge = commits[i];

                    if (!app.IsCommitPresent(org, name, merge))
                    {
                        app.Save(
                            new GitDiff(
                                debug,
                                new GitProcess(
                                    debug,
                                    "git",
                                    new[] {
                                        "rev-list",
                                        $"{merge}~",
                                    },
                                    path.FullName
                                ).Output().First(),
                                merge,
                                new GitLastMajorUpdateTag(debug, path.FullName, merge).Sha(),
                                path.FullName,
                                name,
                                null,
                                org,
                                DateTimeOffset.UtcNow,
                                gitPaths
                            )
                        );

                        output.WriteLine($"{merge} {i}/{(commits.Count - 2)} added");
                    }
                    else
                    {
                        output.WriteLine($"{merge} {i}/{(commits.Count - 2)} skipped");
                    }
                }
            },
            pathOption,
            orgOption,
            nameOption,
            verboseOption,
            gitPathspecArg
        );

        var updateByOneCommand = new Command("update-by-one", "Update the rating by analyzing a merge commit");

        updateByOneCommand.Add(pathOption);
        updateByOneCommand.Add(commitArg);
        updateByOneCommand.Add(linkOption);
        updateByOneCommand.Add(orgOption);
        updateByOneCommand.Add(nameOption);
        updateByOneCommand.Add(timeOption);
        updateByOneCommand.Add(verboseOption);
        updateByOneCommand.Add(gitPathspecArg);
        updateByOneCommand.SetHandler(
            (path, merge, link, org, name, time, verbose, gitPaths) =>
            {
                var debug = verbose == true ? loggerFactory : new LoggerFactory();

                var diff = new GitDiff(
                    debug,
                    new GitProcess(
                        debug,
                        "git",
                        new[] {
                            "rev-parse",
                            $"{merge}~"
                        },
                        path.FullName).Output().First(),
                    new GitProcess(
                        debug,
                        "git",
                        new[] {
                            "rev-parse",
                            merge
                        },
                        path.FullName).Output().First(),
                    new GitLastMajorUpdateTag(debug, path.FullName, merge).Sha(),
                    path.FullName,
                    name ?? "unnamed",
                    link,
                    org ?? "none",
                    time ?? DateTimeOffset.UtcNow,
                    gitPaths
                );

                var app = Application(debug);

                app.Save(diff);

                app.Print(output, diff);
            },
            pathOption,
            commitArg,
            linkOption,
            orgOption,
            nameOption,
            timeOption,
            verboseOption,
            gitPathspecArg
        );

        var serializeCommand = new Command("serialize", "Serialize commit metadata");
        serializeCommand.Add(pathOption);
        serializeCommand.Add(commitArg);
        serializeCommand.Add(linkOption);
        serializeCommand.Add(orgOption);
        serializeCommand.Add(nameOption);
        serializeCommand.Add(timeOption);
        serializeCommand.Add(verboseOption);
        serializeCommand.Add(gitPathspecArg);
        serializeCommand.SetHandler(
            (path, merge, link, org, name, time, verbose, gitPaths) =>
            {
                System.Console.WriteLine(gitPaths);

                var debug = verbose == true ? loggerFactory : new LoggerFactory();

                output.WriteLine(
                    new GitDiff(
                        debug,
                    new GitProcess(
                        debug,
                        "git",
                        new[] {
                            "rev-parse",
                            $"{merge}~"
                        },
                        path.FullName
                    ).Output().First(),
                    new GitProcess(
                        debug,
                        "git",
                        new[] {
                            "rev-parse",
                            merge
                        },
                        path.FullName
                    ).Output().First(),
                    new GitLastMajorUpdateTag(debug, path.FullName, merge).Sha(),
                        path.FullName,
                        name ?? "unnamed",
                        link,
                        org ?? "none",
                        time ?? DateTimeOffset.UtcNow,
                        gitPaths
                    )
                    .ToJson()
                );
            },
            pathOption,
            commitArg,
            linkOption,
            orgOption,
            nameOption,
            timeOption,
            verboseOption,
            gitPathspecArg
        );

        var showCommand = new Command("show", "Print previously applied commit details");
        showCommand.Add(commitArg);
        showCommand.Add(orgOption);
        showCommand.Add(nameOption);
        showCommand.SetHandler(
            (head, org, name) =>
            {
                Application(new LoggerFactory()).Print(output, new ThinDiff(org ?? "none", name ?? "unnamed", head));
            },
            commitArg,
            orgOption,
            nameOption
        );


        var applyCommand = new Command("apply", "Deserialize commit metadata and update the rating");
        applyCommand.Add(jsonOption);
        applyCommand.SetHandler(
            (json) =>
            {
                var diff = new JsonDiff(json);

                var app = Application(new LoggerFactory());

                app.Save(diff);

                app.Print(output, diff);
            },
            jsonOption
        );

        var topCommand = new Command("top", "Print the rating of active contributors for the last 90 days");
        topCommand.Add(orgOption);
        topCommand.Add(nameOption);
        topCommand.SetHandler(
            (org, name) =>
            {
                Application(new LoggerFactory()).Top(output, org ?? "none", name ?? "unnamed");
            },
            orgOption,
            nameOption
        );

        var rootCommand = new RootCommand(
            "Dev Rating suggests the optimal Pull Request size for each contributor " +
            "so that the PRs will have the same impact to the codebase."
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
