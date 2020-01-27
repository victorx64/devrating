![](https://github.com/victorx64/devrating/workflows/build/badge.svg)
[![NuGet](https://img.shields.io/nuget/v/DevRating.ConsoleApp.svg)](https://www.nuget.org/packages/DevRating.ConsoleApp/)
[![codecov](https://codecov.io/gh/victorx64/devrating/branch/master/graph/badge.svg)](https://codecov.io/gh/victorx64/devrating)

**Dev Rating** is a command-line tool that ranks developers by the stability of their code. 

Read [how it works](docs/how-it-works.md). Read the 
[white paper](docs/white-paper.md) (in [russian](docs/white-paper-ru.md)).

Also, check [Dev Rating](https://github.com/marketplace/dev-rating) app for 
GitHub that automatically analyzes pull requests. 

First, install the tool:

```
$ dotnet tool install -g devrating.consoleapp
```

Then, you run it and follow the instructions:

```
$ devrating
```

It should be clear what to do. If not, ask us in
our [Telegram chat](https://t.me/devratingchat).

## Reward

Print a reward for the work between commits:

```
$ devrating show <path-to-repo> <commit> <commit>
```

## Rating

The rating is stored in the `devrating.db` file in a working directory.

Update the rating by committing the work between commits:

```
$ devrating add <path-to-repo> <commit> <commit>
```

Print the rating:

```
$ devrating top
```

## How to contribute

Fork repository, make changes, send us a pull request. We will review
your changes and apply them to the `master` branch shortly, provided
they don't violate our quality standards.
