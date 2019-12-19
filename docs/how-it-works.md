## How DevRating works

Read the 
[white paper](docs/white-paper.md) (in [russian](docs/white-paper-ru.md)).

### TL;DR

Every single line deletion gives rating points to an author and lowers points 
of the deleted line author. Every added line gives a reward to its author 
proportionally to his rating.

### More details

DevRating evaluates a reward of a developer for a work. Work is described as 
a number of added and deleted lines of code between two commits. It's similar 
to what git diff shows. An author of the second commit is treated as the 
developer of the work.

First, DevRating counts a reward based on a number of added lines and on the 
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

Then, DevRating updates the ratings.

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
