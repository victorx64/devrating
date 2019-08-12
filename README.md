[![NuGet](https://img.shields.io/nuget/v/DevRating.Console.svg)](https://www.nuget.org/packages/DevRating.Console/)

# DevRating

Calculates developers rating based on a history of files modifications. Works in a git repository. Every code line modification gives points to a modifier and lowers points of the previous author of the line. Uses [Elo rating system](https://en.wikipedia.org/wiki/Elo_rating_system).

Creating a new file doesn't affect anything. Removing a file gives points. Ignores merge commits.

## Prerequisites
1. [.NET Core 2.1](https://dotnet.microsoft.com/download/dotnet-core/2.1) runtime
2. [Git](https://git-scm.com/downloads) client

## Install
```Batchfile
dotnet tool install -g DevRating.Console --version 0.1.3
```

## Run
Move to a repository that you want to inspect. Run `devrating`:

```Batchfile
devrating
```

It can take minutes to get done. The result will be printed after `Author, Wins, Defeats, Points` line. Every developer starts with `1200` points. `Wins` shows the number of lines modified by this person, and `Defeats` is number of lines of this developer modified by another member.
