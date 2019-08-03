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

            var files = new Dictionary<string, IFile>();

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

        private IPlayers UpdateRating(IDictionary<string, IFile> files, string author, Patch differences,
            IPlayers developers)
        {
            var before = new Dictionary<string, IFile>(files);

            foreach (var difference in differences)
            {
                var previous = difference.Status == ChangeKind.Added
                    ? new File()
                    : before[difference.OldPath];

                var binary = difference.IsBinaryComparison ||
                             difference.Status == ChangeKind.TypeChanged;

                var file = previous.PatchedFile(binary, author, difference.Patch);

                developers = file.UpdatedPlayers(developers);

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

            return developers;
        }
    }
}