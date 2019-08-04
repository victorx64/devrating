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

            IDictionary<string, IFile> files = new Dictionary<string, IFile>();

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

                Tree tree = null;

                foreach (var current in repo.Commits.QueryBy(filter))
                {
                    var differences = repo.Diff.Compare<Patch>(tree, current.Tree, options);

                    developers = UpdatedDevelopers(files, differences, developers);

                    files = UpdatedFiles(files, current.Author.Email, differences);

                    tree = current.Tree;
                }
            }

            return developers;
        }

        private IDictionary<string, IFile> UpdatedFiles(IDictionary<string, IFile> files, string author,
            Patch differences)
        {
            var after = new Dictionary<string, IFile>(files);

            foreach (var difference in differences)
            {
                if (difference.Status != ChangeKind.Added &&
                    difference.Status != ChangeKind.Copied)
                {
                    after.Remove(difference.OldPath);
                }

                if (difference.Status != ChangeKind.Deleted)
                {
                    var binary = difference.IsBinaryComparison ||
                                 difference.Status == ChangeKind.TypeChanged;

                    var previous = difference.Status == ChangeKind.Added ? new File() : files[difference.OldPath];

                    var file = previous.PatchedFile(binary, author, difference.Patch);

                    after.Add(difference.Path, file);
                }
            }

            return after;
        }

        private IPlayers UpdatedDevelopers(IDictionary<string, IFile> files, Patch differences, IPlayers developers)
        {
            foreach (var difference in differences)
            {
                developers = files[difference.Path]
                    .UpdatedDevelopers(developers);
            }

            return developers;
        }
    }
}