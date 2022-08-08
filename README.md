[![NuGet](https://img.shields.io/nuget/v/devrating.consoleapp.svg)](https://www.nuget.org/packages/devrating.consoleapp/)
[![Downloads](https://img.shields.io/nuget/dt/devrating.consoleapp.svg)](https://www.nuget.org/packages/devrating.consoleapp/)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=victorx64_devrating&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=victorx64_devrating)

**Dev Rating** is a command-line tool that suggests minimal Pull Request size for each contributor so that the PRs will have the same impact to the codebase. Having PRs with the same impact makes it easier to evaluate the amount of work done.

It counts the deleted lines of code in the PRs and builds a contributor rating based on code stability. Low-rated members are encouraged to post more code in PR because their code statistically lives less.

Merge commits authors treated as authors of the PRs. In the case of rebased PRs, it treats each commit as a separate PR.

# Usage

Install the tool:

```
dotnet tool install -g devrating.consoleapp
```

Analyze your repository:

```
cd <path-to-repo>

git checkout master

devrating update
```

Print the suggested PR sizes for each contributor:

```
devrating top
```

Sample output:

```
Rating  | PRs (90d) | Suggested additions | Author
------- | --------- | ------------------- | ------
1553.86 |        45 |                  21 | <546546+cassidy@users.noreply.github.com>
1524.85 |        10 |                  23 | <5558798+ash@users.noreply.github.com>
1513.59 |        13 |                  24 | <94203766+bot@users.noreply.github.com>
1499.43 |         0 |                  25 | <lena@gmail.com>
1496.14 |         0 |                  25 | <46346212+ghost@users.noreply.github.com>
1495.48 |         0 |                  25 | <viktor_semenov@outlook.com>
1485.74 |         0 |                  26 | <453474572+bob@users.noreply.github.com>
1481.63 |        96 |                  26 | <alice@gmail.com>
1475.78 |        12 |                  27 | <john@outlook.com>
1473.52 |        77 |                  27 | <uri@live.com>
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

This repository is a [Software Quality Award](https://www.yegor256.com/2019/11/03/award-2020.html) winner.
