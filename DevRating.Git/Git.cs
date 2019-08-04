using System.Collections.Generic;
using DevRating.Rating;
using LibGit2Sharp;

namespace DevRating.Git
{
    public sealed class Git
    {
        private readonly IList<IPlayer> _developers;

        public Git(IList<IPlayer> developers)
        {
            _developers = developers;
        }

        public IList<IPlayer> Developers()
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

                    files = PatchedFiles(files, current.Author.Email, differences);

                    developers = UpdatedDevelopers(files, differences, developers);

                    files = MovedFiles(files, differences);

                    tree = current.Tree;
                }
            }

            return developers;
        }

        private IDictionary<string, IFile> PatchedFiles(IDictionary<string, IFile> files, string author,
            Patch differences)
        {
            var result = new Dictionary<string, IFile>(files);

            foreach (var difference in differences)
            {
                if (difference.Status == ChangeKind.Added)
                {
                    continue;
                }

                var binary = difference.IsBinaryComparison ||
                             difference.Status == ChangeKind.TypeChanged;

                files[difference.OldPath] = files[difference.OldPath].PatchedFile(binary, author, difference.Patch);
            }

            return result;
        }

        private IDictionary<string, IFile> MovedFiles(IDictionary<string, IFile> files, Patch differences)
        {
            var result = new Dictionary<string, IFile>(files);

            foreach (var difference in differences)
            {
                switch (difference.Status)
                {
                    case ChangeKind.Added:
                        result.Add(difference.Path, new File());
                        break;
                    case ChangeKind.Deleted:
                        result.Remove(difference.OldPath);
                        break;
                    case ChangeKind.Renamed:
                        result.Remove(difference.OldPath, out var file);
                        result.Add(difference.Path, file);
                        break;
                    case ChangeKind.Copied:
                        result.Add(difference.Path, files[difference.OldPath]);
                        break;
                }
            }

            return result;
        }

        private IList<IPlayer> UpdatedDevelopers(IDictionary<string, IFile> files, Patch differences, IList<IPlayer> developers)
        {
            foreach (var difference in differences)
            {
                developers = files[difference.OldPath]
                    .UpdatedDevelopers(developers);
            }

            return developers;
        }
    }
}