using System.Collections.Generic;
using DevRating.Rating;
using LibGit2Sharp;

namespace DevRating.Git
{
    public sealed class Git : IGit
    {
        private readonly IDictionary<string, IPlayer> _authors;
        private readonly IPlayer _author;
        private readonly IFile _file;

        public Git(IDictionary<string, IPlayer> authors, IPlayer author, IFile file)
        {
            _authors = authors;
            _author = author;
            _file = file;
        }

        public IDictionary<string, IPlayer> Authors()
        {
            var authors = _authors;

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

                    files = WithAddedFiles(files, differences);

                    files = PatchedFiles(files, current.Author.Email, differences);

                    authors = UpdatedAuthors(files, authors, differences);

                    files = WithoutRemovedFiles(files, differences);

                    tree = current.Tree;
                }
            }

            return authors;
        }

        private IDictionary<string, IFile> WithAddedFiles(IDictionary<string, IFile> files, Patch differences)
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

        private IDictionary<string, IFile> PatchedFiles(IDictionary<string, IFile> files, string author,
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

        private IDictionary<string, IPlayer> UpdatedAuthors(IDictionary<string, IFile> files,
            IDictionary<string, IPlayer> authors, Patch differences)
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

        private IDictionary<string, IFile> WithoutRemovedFiles(IDictionary<string, IFile> files, Patch differences)
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