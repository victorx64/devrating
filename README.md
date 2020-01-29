![](https://github.com/victorx64/devrating/workflows/build/badge.svg)
[![NuGet](https://img.shields.io/nuget/v/DevRating.ConsoleApp.svg)](https://www.nuget.org/packages/DevRating.ConsoleApp/)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=victorx64_devrating&metric=coverage)](https://sonarcloud.io/dashboard?id=victorx64_devrating)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=victorx64_devrating&metric=alert_status)](https://sonarcloud.io/dashboard?id=victorx64_devrating)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=victorx64_devrating&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=victorx64_devrating)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=victorx64_devrating&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=victorx64_devrating)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=victorx64_devrating&metric=bugs)](https://sonarcloud.io/dashboard?id=victorx64_devrating)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=victorx64_devrating&metric=sqale_index)](https://sonarcloud.io/dashboard?id=victorx64_devrating)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=victorx64_devrating&metric=code_smells)](https://sonarcloud.io/dashboard?id=victorx64_devrating)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=victorx64_devrating&metric=vulnerabilities)](https://sonarcloud.io/dashboard?id=victorx64_devrating)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=victorx64_devrating&metric=duplicated_lines_density)](https://sonarcloud.io/dashboard?id=victorx64_devrating)

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
