using System.Collections.Generic;
using DevRating.Rating;
using LibGit2Sharp;

namespace DevRating.Git
{
    public sealed class Git : AuthorsCollection
    {
        private readonly IDictionary<string, Player> _authors;
        private readonly Player _author;
        private readonly File _file;

        public Git(IDictionary<string, Player> authors, Player author, File file)
        {
            _authors = authors;
            _author = author;
            _file = file;
        }

        public IDictionary<string, Player> Authors()
        {
            var authors = _authors;

            IDictionary<string, File> files = new Dictionary<string, File>();

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

                    files = WithAddedFiles(files, differences);

                    files = PatchedFiles(files, current.Author.Email, differences);

                    authors = UpdatedAuthors(files, authors, differences);

                    files = WithoutRemovedFiles(files, differences);

                    tree = current.Tree;
                }
            }

            return authors;
        }

        private IDictionary<string, File> WithAddedFiles(IDictionary<string, File> files, Patch differences)
        {
            var result = files; // new Dictionary<string, IFile>(files);

            foreach (var difference in differences)
            {
                if (difference.Status == ChangeKind.Added)
                {
                    result.Add(difference.Path, _file);
                }

                if (difference.Status == ChangeKind.Renamed ||
                    difference.Status == ChangeKind.Copied)
                {
                    result.Add(difference.Path, result[difference.OldPath]);
                }
            }

            return result;
        }

        private IDictionary<string, File> PatchedFiles(IDictionary<string, File> files, string author,
            Patch differences)
        {
            var result = files; // new Dictionary<string, IFile>(files);

            foreach (var difference in differences)
            {
                var binary = difference.IsBinaryComparison ||
                             difference.Status == ChangeKind.TypeChanged;

                result[difference.Path] = result[difference.Path].PatchedFile(binary, author, difference.Patch);
            }

            return result;
        }

        private IDictionary<string, Player> UpdatedAuthors(IDictionary<string, File> files,
            IDictionary<string, Player> authors, Patch differences)
        {
            foreach (var difference in differences)
            {
                var changes = files[difference.Path].ChangedAuthors();

                foreach (var change in changes)
                {
                    authors = change.UpdatedAuthors(authors, _author);
                }
            }

            return authors;
        }

        private IDictionary<string, File> WithoutRemovedFiles(IDictionary<string, File> files, Patch differences)
        {
            var result = files; // new Dictionary<string, IFile>(files);

            foreach (var difference in differences)
            {
                if (difference.Status == ChangeKind.Deleted ||
                    difference.Status == ChangeKind.Renamed)
                {
                    result.Remove(difference.OldPath);
                }
            }

            return result;
        }
    }
}