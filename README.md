[![NuGet](https://img.shields.io/nuget/v/DevRating.ConsoleApp.svg)](https://www.nuget.org/packages/DevRating.ConsoleApp/)

**DevRating** is a command-line tool for measuring a reward of developers 
based on a git log. Every single line deletion gives rating points to 
an author and lowers points of the deleted line author. 
It uses the [Elo rating system](https://en.wikipedia.org/wiki/Elo_rating_system). 
Every added line gives a reward to its author proportionally to his rating.

First, use [dotnet](https://dotnet.microsoft.com/download/dotnet-core) 
to install the app:

```
$ dotnet tool install -g devrating.consoleapp
```

Then, you run it and follow the instructions:

```
$ devrating
```

It should be clear what to do. If not, ask us in
our [Telegram chat](https://t.me/devrating).

## Rating updates

Print *reward* and *rating updates* made by changes between commits:

```
$ devrating show <path-to-repo> <commit> <commit>
```

Update the leaderboard by committing changes between commits:

```
$ devrating add <path-to-repo> <commit> <commit>
```

## Leaderboard

Print the leaderboard:

```
$ devrating top
```

The leaderboard is stored in the `devrating.db` file in a working directory.

## How to contribute

Fork repository, make changes, send us a pull request. We will review
your changes and apply them to the `master` branch shortly, provided
they don't violate our quality standards.
