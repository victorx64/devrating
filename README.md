![](https://github.com/victorx64/devrating/workflows/build/badge.svg)
[![NuGet](https://img.shields.io/nuget/v/DevRating.ConsoleApp.svg)](https://www.nuget.org/packages/DevRating.ConsoleApp/)
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

**Dev Rating** is a command-line tool that counts deleted and added lines of 
code between commits and evaluates a reward for the changes.

Read the [white paper](docs/white-paper.md) 
(или [по-русски](docs/white-paper-ru.md)).

Also, check the [GitHub Action](https://github.com/victorx64/devrating-gh-action)
that evaluates rewards for the pull requests. 

# Usage

## Installation

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

## Print a reward for a work

First, update the rating:

```
$ devrating add <path-to-repo> <commit> <commit>
```

The rating is needed to properly evaluate a reward of a developer for a work.
The rating is stored in the `devrating.db` file in a working directory.

Then, print the reward: 

```
$ devrating show <path-to-repo> <commit> <commit>
```

## Print the rating

```
$ devrating top
```

# How it works

> The metric you chose, especially the lines of code removed part, is essential 
for someone to establish your "level" as a coder. 
I think that is an excellent measure.

<p align="right">— prof David West, <i>author of Object Thinking</i></p>

## TL;DR

Every single line deletion gives rating points to an author and lowers points 
of the deleted line author. Every added line gives a reward to its author 
proportionally to his rating.

## More details

Dev Rating evaluates a reward of a developer for a work. Work is described as 
a number of added and deleted lines of code between two commits. It's similar 
to what git diff shows. An author of the second commit is treated as the 
developer of the work.

First, Dev Rating counts a reward based on a number of added lines and on the 
current rating of the developer:

```
r = c * l / (1 - p)
```

where `r` - reward, `c` - free coefficient, `l` - number of added lines, 
`p` - the probability of winning of the developer against a developer with an 
average rating. In the current version `c = 1`. In future releases, you will 
be able to specify it with a config file in the repository.

Evaluation of `p`:

```
Qa = 10 ^ (a / 400);
Qb = 10 ^ (b / 400);
p = Qa / (Qa   Qb);
```

where `a` - rating of the developer, `b` - average rating, which is currently 
`1500`.

Then, Dev Rating updates the ratings.

When a developer deletes a line, he increases his rating and lowers the 
rating of the deleted line author. The 
[Elo rating system](https://en.wikipedia.org/wiki/Elo_rating_system) 
is used with the following constants:

```
k = 2;
n = 400;
```

When the system meets a new author it sets `1500` rating points to him. 
This is an average rating of the system.

## How is this better than the lines-of-code metric?

The rating system fixes the known
[disadvantages](https://en.wikipedia.org/wiki/Source_lines_of_code#Disadvantages)
of the LoC metric:

### Lack of accountability

> lines-of-code measure suffers from some fundamental 
problems. Some think that it isn't useful to measure the productivity of a 
project using only results from the coding phase, which usually accounts for 
only 30% to 35% of the overall effort.

The overall effort must be applied to increase the stability of the code.
Dev Rating measures the stability of code and uses it to evaluate a reward 
for work.

### Lack of cohesion with functionality

> though experiments have repeatedly confirmed that while effort is highly 
correlated with LOC, functionality is less well correlated with LOC. That is, 
skilled developers may be able to develop the same functionality with far less
code, so one program with less LOC may exhibit more functionality than another
similar program. In particular, LOC is a poor productivity measure of
individuals, because a developer who develops only a few lines may still be
more productive than a developer creating more lines of code – even more: some
good refactoring like "extract method" to get rid of redundant code and keep
it clean will mostly reduce the lines of code.

Writing functionality with far less code may harm the readability.
The main goal is to motivate programmers to write highly readable code.
To make it readable code must have uniform complexity. Dev Rating 
encourages decomposing complex logic into several simple lines. 

### Adverse impact on estimation

> because of the fact presented under point #1,
estimates based on lines of code can adversely go wrong, in all possibility.

Having code with uniform complexity and expected stability allows us to make 
more accurate estimations and to measure the actual performance.

### Developer's experience

> implementation of a specific logic differs based on 
the level of experience of the developer. Hence, number of lines of code 
differs from person to person. An experienced developer may implement certain 
functionality in fewer lines of code than another developer of relatively less 
experience does, though they use the same language.

A developer with a high rating may get more rewards for fewer lines of code
than another developer with a lower rating for more lines of code.

### Difference in languages

> consider two applications that provide the same 
functionality (screens, reports, databases). One of the applications is written
in C++ and the other application written in a language like COBOL. The number 
of function points would be exactly the same, but aspects of the application 
would be different. The lines of code needed to develop the application would
certainly not be the same. As a consequence, the amount of effort required to
develop the application would be different (hours per function point). Unlike
lines of code, the number of function points will remain constant.

True. Different languages have different expressiveness. The solution 
architect's task is to choose the right language for the project. It should be
expressive enough but highly readable to maximize the effectiveness of 
programmers.

### Advent of GUI tools

> with the advent of GUI-based programming languages and 
tools such as Visual Basic, programmers can write relatively little code and 
achieve high levels of functionality. For example, instead of writing a program
to create a window and draw a button, a user with a GUI tool can use
drag-and-drop and other mouse operations to place components on a workspace.
Code that is automatically generated by a GUI tool is not usually taken into
consideration when using LOC methods of measurement. This results in variation
between languages; the same task that can be done in a single line of code (or
no code at all) in one language may require several lines of code in another.

The same answer as for the previous point. The solution architect's task is to
choose the right toolset to maximize the effectiveness of programmers and minimize
the cost of development.

### Problems with multiple languages

> in today's software scenario, software is 
often developed in more than one language. Very often, a number of languages 
are employed depending on the complexity and requirements. Tracking and 
reporting of productivity and defect rates poses a serious problem in this 
case, since defects cannot be attributed to a particular language subsequent 
to integration of the system. Function point stands out to be the best measure 
of size in this case.

Since Dev Rating counts line deletions instead of defect rates it can measure
the stability of code in a particular language of a particular programmer.
The solution architect may set different base cost per line for different 
languages in a project.

### Lack of counting standards

> there is no standard definition of what a line 
of code is. Do comments count? Are data declarations included? What happens 
if a statement extends over several lines? – These are the questions that often
arise. Though organizations like SEI and IEEE have published some guidelines in
an attempt to standardize counting, it is difficult to put these into practice
especially in the face of newer and newer languages being introduced every year.

When a programmer reads code he sees every physical line. To take into account
the readability Dev Rating counts every physical line.

### Psychology

> a programmer whose productivity is being measured in lines of
code will have an incentive to write unnecessarily verbose code. The more
management is focusing on lines of code, the more incentive the programmer has
to expand his code with unneeded complexity. This is undesirable, since
increased complexity can lead to increased cost of maintenance and increased
effort required for bug fixing.

The rating system motivates programmers to write stable code and to 
continuously improve codebase by deleting bad code. This approach encourages
them not to write unnecessarily verbose code.

# How to contribute

Fork repository, make changes, send us a pull request. We will review
your changes and apply them to the `master` branch shortly, provided
they don't violate our quality standards.
