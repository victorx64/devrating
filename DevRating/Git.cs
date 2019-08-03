using System;
using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;

namespace DevRating
{
    public class Git : IPlayersHub
    {
        private readonly IPlayers _players;

        public Git(IPlayers players)
        {
            _players = players;
        }

        public IPlayers Players()
        {
            var rating = _players;

            var files = new Dictionary<string, File>();

            using (var repo = new Repository("."))
            {
                var options = new CompareOptions()
                {
                    ContextLines = 0
                };

                var filter = new CommitFilter
                {
                    SortBy = CommitSortStrategies.Topological |
                             CommitSortStrategies.Reverse
                };

                var index = 0;
                var count = repo.Commits.QueryBy(filter).Count();

                Tree tree = null;

                foreach (var current in repo.Commits.QueryBy(filter))
                {
                    Console.WriteLine($"{++index} / {count}");

                    var differences = repo.Diff.Compare<Patch>(tree, current.Tree, options);

                    rating = UpdateRating(files, current.Author.Email, differences, rating);

                    tree = current.Tree;
                }
            }

            return rating;
        }

        private IPlayers UpdateRating(IDictionary<string, File> files, string author, Patch differences, IPlayers players)
        {
            var before = new Dictionary<string, File>(files);

            foreach (var difference in differences)
            {
                var previous = difference.Status == ChangeKind.Added
                    ? new File(new string[0], false)
                    : before[difference.OldPath];

                var binary = difference.IsBinaryComparison ||
                             difference.Status == ChangeKind.TypeChanged;

                var file = UpdateFile(previous, author, difference.Patch, binary);

                players = file.UpdateRating(players);

                if (difference.Status != ChangeKind.Added &&
                    difference.Status != ChangeKind.Copied)
                {
                    files.Remove(difference.OldPath);
                }

                if (difference.Status != ChangeKind.Deleted)
                {
                    files.Add(difference.Path, file);
                }
            }

            return players;
        }

        private File UpdateFile(File previous, string author, string patch, bool binary)
        {
            var file = new File(previous.Authors(), previous.Binary() || binary);

            if (!binary)
            {
                var lines = patch.Split('\n');

                foreach (var line in lines)
                {
                    if (line.StartsWith("@@ "))
                    {
                        var parts = line.Split(' ');

                        file.AddDeletion(new LinesBlock(author, parts[1]));
                        file.AddAddition(new LinesBlock(author, parts[2]));
                    }
                }
            }

            return file;
        }
    }
}