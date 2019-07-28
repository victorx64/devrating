# DevRating

Calculates developers rating based on a history of files modifications. Works in a git repository. Every code line modification gives points to a modifier and lowers points of the previous author of the line. Uses [Elo rating system](https://en.wikipedia.org/wiki/Elo_rating_system).

Creating a new file doesn't affect anything. Removing a file gives points.

## Install

```Batchfile
dotnet tool install -g devrating
```

## Run
Move to a repository that you want to inspect. Run `devrating`:

```Batchfile
devrating
```

It can take minutes to get done. The result will be printed after `Player, Wins, Defeats, Points` line. Every developer starts with `1200` points. `Wins` shows the number of lines modified by this person, and `Defeats` is number of lines of this developer modified by another member.
