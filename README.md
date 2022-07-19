[![NuGet](https://img.shields.io/nuget/v/devrating.consoleapp.svg)](https://www.nuget.org/packages/devrating.consoleapp/)
[![Downloads](https://img.shields.io/nuget/dt/devrating.consoleapp.svg)](https://www.nuget.org/packages/devrating.consoleapp/)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=victorx64_devrating&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=victorx64_devrating)

**Dev Rating** is a command-line tool that suggests minimal Pull Request size for each contributor so that the PRs will have the same impact to the codebase. Having PRs with the same impact makes it easier to evaluate the amount of work done.

It counts the deleted lines of code in the PRs and builds a contributor rating based on code stability. Low-rated members are encouraged to post more code in PR because their code statistically lives less.

Merge commits authors treated as authors of the PRs. In the case of rebased PRs, it treats each commit as a separate PR.

# Usage

Install the tool:

```
$ dotnet tool install -g devrating.consoleapp
```

Add a PR merge commit:

```
$ devrating add -p <path-to-repo> -m <merge-commit>
```

Where:
- `<path-to-repo>` — path to a local git repository.
- `<merge-commit>` — a merge or squash commit of a merged PR.

It updates the rating. Repeat this step for other PRs in historical order. The rating is stored in `devrating.sqlite3` SQLite file in a working directory.

Print the suggested PR sizes for each contributor:

```
$ devrating top
```

Sample output:

```
Author                       | Rating  | Minimal additions in PR
---------------------------- | ------- | -----------------------
<svikk@live.ru>              | 1520.00 | 24
<viktor_semenov@outlook.com> | 1480.00 | 26
```

# How it works

When Developer A deletes lines of code, he increases his rating and lowers the 
rating of the deleted lines author (Developer B).
The [Elo rating system](https://en.wikipedia.org/wiki/Elo_rating_system) is used:

$K = 40; N = 400; $

$Q_A = 10 ^ {\frac{R_A}{N}}; $  
$Q_B = 10 ^ {\frac{R_B}{N}}; $

$E_{A,B} = \frac{Q_A}{Q_A + Q_B}; $

$R_\Delta = K(1 - E_{A,B})(\sum_{i = 1} \frac{Del_{Ai}}{Add_{Bi}}); $

$R_A^{'} = R_A + R_\Delta; $  
$R_B^{'} = R_B - R_\Delta; $

where
- $Add_{Bi}$ - number of added lines by Developer B in the $i$-th PR,
- $Del_{Ai}$ - number of deleted lines by Developer A from the $i$-th PR,
- $R_A$ - initial rating of Developer A,
- $R_B$ - initial rating of Developer B,
- $R_A^{'}$ - new rating of Developer A,
- $R_B^{'}$ - new rating of Developer B.

When the system meets a new author it sets $1500$ rating points to him.

Minimal additions per PR:

$Q_{avg} = 10 ^ {\frac{1500}{N}}; $

$E_{A,avg} = \frac{Q_A}{Q_A + Q_{avg}}; $

$M_A = 50 (1 - E_{A,avg}); $

where
- $M_A$ - the minimal added lines number for Developer A in his PRs.

An average-rated member suggested to add 25 lines of code in PRs.

## Ignoring too old lines

The tool doesn't change rating if deleted line was introduced in previous major versions of code. It reads git tags with `semver` to figure out when was the last major update.

# Build and run

```
dotnet run --project ./consoleapp
```

<a href="https://www.yegor256.com/2019/11/03/award-2020.html">
  <img src="https://www.yegor256.com/images/award/2020/winner-victorx64.png" height="75" alt='winner'/>
</a>
