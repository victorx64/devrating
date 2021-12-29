![](https://github.com/victorx64/devrating/workflows/build/badge.svg)
[![NuGet](https://img.shields.io/nuget/v/DevRating.ConsoleApp.svg)](https://www.nuget.org/packages/DevRating.ConsoleApp/)
[![Downloads](https://img.shields.io/nuget/dt/DevRating.ConsoleApp.svg)](https://www.nuget.org/packages/DevRating.ConsoleApp/)
[![Lines](https://img.shields.io/tokei/lines/github/victorx64/devrating)]()
[![Hits-of-Code](https://hitsofcode.com/github/victorx64/devrating)](https://hitsofcode.com/view/github/victorx64/devrating)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=victorx64_devrating&metric=coverage)](https://sonarcloud.io/dashboard?id=victorx64_devrating)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=victorx64_devrating&metric=alert_status)](https://sonarcloud.io/dashboard?id=victorx64_devrating)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=victorx64_devrating&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=victorx64_devrating)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=victorx64_devrating&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=victorx64_devrating)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=victorx64_devrating&metric=bugs)](https://sonarcloud.io/dashboard?id=victorx64_devrating)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=victorx64_devrating&metric=sqale_index)](https://sonarcloud.io/dashboard?id=victorx64_devrating)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=victorx64_devrating&metric=code_smells)](https://sonarcloud.io/dashboard?id=victorx64_devrating)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=victorx64_devrating&metric=vulnerabilities)](https://sonarcloud.io/dashboard?id=victorx64_devrating)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=victorx64_devrating&metric=duplicated_lines_density)](https://sonarcloud.io/dashboard?id=victorx64_devrating)

<a href="https://www.yegor256.com/2019/11/03/award-2020.html">
  <img src="https://www.yegor256.com/images/award/2020/winner-victorx64.png" height="75" alt='winner'/>
</a>

**Dev Rating** is a command-line tool that suggests the optimal Pull Request size for each contributor of a repository so that PRs will have the same overall durability of added lines of code.

It counts the deleted lines of code in previous PRs and builds a contributor rating based on code stability. Low-ranked members are encouraged to post more code in PRs because their code statistically lives less.

Having PRs with the same stability makes it easy to calculate how much work has been done.

# Usage with .NET

Install the tool:

```
$ dotnet tool install -g devrating.consoleapp
```

Add a previous PR:

```
$ devrating add commit -p <path-to-repo> -m <merge-commit>
```

Where:
- `<path-to-repo>` — path to a local git repository.
- `<merge-commit>` — a merge or squash commit of a merged PR.

It updates the rating. Repeat this step for other PRs in historical order. The rating is stored in `devrating.sqlite3` SQLite file in a working directory.

Print the suggested PR sizes for each contributor:

```
$ devrating top
```

# Usage with Docker

Add a previous PR:

```
$ docker run -it --rm -v <path-to-repo>:/repo -v <working-dir>:/workspace victorx64/devrating:latest add commit -p /repo -m <merge-commit>
```

Where:
- `<path-to-repo>` — path to a local git repository.
- `<working-dir>` — path where the rating will be stored as `devrating.sqlite3` SQLite file.
- `<merge-commit>` — a merge or squash commit of a merged PR.

It updates the rating. Repeat this step for other PRs in historical order. The rating is stored in `devrating.sqlite3` SQLite file in a working directory.

Print the suggested PR sizes for each contributor:

```
$ docker run -it --rm -v <path-to-repo>:/repo -v <working-dir>:/workspace victorx64/devrating:latest top
```

Where:
- `<path-to-repo>` — path to a local git repository.
- `<working-dir>` — path where the rating is stored as `devrating.sqlite3` SQLite file.

# How it works

When Developer A deletes a line of code, he increases his rating and lowers the 
rating of the deleted line author (Developer B).
The [Elo rating system](https://en.wikipedia.org/wiki/Elo_rating_system) is used:

```
k = 40;
n = 400;

qA = 10 ^ (ratingA / n);
qB = 10 ^ (ratingB / n);
eA = qA / (qA + qB);
d = k * (1 - eA) * deletionsA / additionsB;

newRatingA = ratingA + d;
newRatingB = ratingB - d;
```

where
  - `additionsB` - number of added lines by Developer B in a PR,
  - `deletionsA` - number of deleted lines by Developer A from the PR,
  - `ratingA` - initial rating of Developer A,
  - `ratingB` - initial rating of Developer B,
  - `newRatingA` - new rating of Developer A,
  - `newRatingB` - new rating of Developer B.

When the system meets a new author it sets `1500` rating points to him.
This is an average rating of the system.

# Build and run

```
dotnet run --project ./ConsoleApp
```
