using System;
using System.Collections.Generic;
using System.Linq;
using DevRating.Rating;
using LibGit2Sharp;

namespace DevRating.Git
{
    public sealed class Git
    {
        private readonly IPlayers _developers;

        public Git(IPlayers developers)
        {
            _developers = developers;
        }

        public IPlayers Developers()
        {
            var developers = _developers;

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

                    developers = UpdateRating(files, current.Author.Email, differences, developers);

                    tree = current.Tree;
                }
            }

            return developers;
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

                players = file.UpdatedPlayers(players);

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

                        file.AddDeletion(new DeletionHunk(author, parts[1]));
                        file.AddAddition(new AdditionHunk(author, parts[2]));
                    }
                }
            }

            return file;
        }
    }
}